using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetResponder.Protocols
{
    internal static class NetBIOSHelpers
    {
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
                    // TODO : Should alsp filter non printable characters. Original
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

        internal const int NetbiosNameLength = 32;
    }
}
