using System;
using System.Collections.Generic;

namespace NetResponder.Protocols
{
    internal static class NetBIOSHelpers
    {
        static NetBIOSHelpers()
        {
            _rolesById = new Dictionary<byte[], string>(ByteComparer.Instance);
            _rolesById.Add(new byte[NetbiosRoleIdLength] { 0x41, 0x41, 0x00 }, "Workstation/Redirector");
            _rolesById.Add(new byte[NetbiosRoleIdLength] { 0x42, 0x4c, 0x00 }, "Domain Master Browser");
            _rolesById.Add(new byte[NetbiosRoleIdLength] { 0x42, 0x4d, 0x00 }, "Domain Controller");
            _rolesById.Add(new byte[NetbiosRoleIdLength] { 0x42, 0x4e, 0x00 }, "Local Master Browser");
            _rolesById.Add(new byte[NetbiosRoleIdLength] { 0x42, 0x4f, 0x00 }, "Browser Election");
            _rolesById.Add(new byte[NetbiosRoleIdLength] { 0x43, 0x41, 0x00 }, "File Server");
            _rolesById.Add(new byte[NetbiosRoleIdLength] { 0x41, 0x42, 0x00 }, "Browser");
            return;
        }

        internal static string DecodeName(string nbname)
        {
            try {
                //	from string import printable
                if (NetbiosNameLength != nbname.Length) { return nbname; }
                List<char> resultCharacters = new List<char>();
                for(int index = 0; index < NetbiosNameLength; index += 2) {
                    char candidateChar = (char)(((nbname[index] - 0x41) << 4) | ((nbname[index + 1] - 0x41) & 0x0F));
                    if ('\0' == candidateChar) { break; }
                    if (' ' == candidateChar) { continue; }
                    // TODO : Should also filter non printable characters. Original
                    // implementation is questionable. Filtering occurs after scanning.
                    // A non printable character is allowed in the input and won"t be
                    // considered an illegal name. Is this the intent ?
                    resultCharacters.Add(candidateChar);
                }
                return new string(resultCharacters.ToArray());
            }
            catch (Exception e) {
                throw new ProtocolException("Illegal NetBIOS name", e);
            }
            throw new NotImplementedException();
        }

        internal static string GetRoleById(byte[] id)
        {
            string result;
            return _rolesById.TryGetValue(id, out result) ? result : "Service not known";
        }

        internal const int NetbiosNameLength = 32;
        internal const int NetbiosRoleIdLength = 3;
        internal static readonly Dictionary<byte[], string> _rolesById;

        private class ByteComparer : IEqualityComparer<byte[]>
        {
            static ByteComparer()
            {
                Instance = new ByteComparer();
            }

            private ByteComparer()
            {
            }

            internal static ByteComparer Instance { get; private set;}

            public bool Equals(byte[] x, byte[] y)
            {
                if ((null == x) || (null == y)) { return false; }
                if (x.Length != y.Length) { return false; }
                for(int index = 0; index < x.Length; index++) {
                    if (x[index] != y[index]) { return false; }
                }
                return true;
            }

            public int GetHashCode(byte[] obj)
            {
                int result = 0;
                if (null != obj) {
                    for(int index = 0; index < Math.Min(sizeof(int), obj.Length); index++) {
                        result <<= 8;
                        result += obj[index];
                    }
                }
                return result;
            }
        }
    }
}
