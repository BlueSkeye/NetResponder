using System;
using System.Text;

namespace NetResponder
{
    internal static class Extension
    {
        internal static string Combine(this string[] from, string separator)
        {
            StringBuilder builder = new StringBuilder();
            foreach(string candidate in from) {
                if (0 < builder.Length) { builder.Append(separator); }
                builder.Append(candidate);
            }
            return builder.ToString();
        }

        internal static bool ContentEquals(this byte[] x, byte[] y)
        {
            if (null == y) { return false; }
            if (x.Length != y.Length) { return false; }
            for (int index = 0; index < x.Length; index++) {
                if (x[index] != y[index]) { return false; }
            }
            return true;
        }

        internal static byte[] ExtractData(this byte[] from, int startIndex)
        {
            try { return from.ExtractData(startIndex, from.Length - startIndex); }
            catch (Exception e) {
                // TODO : Remove try/catch once bug's fixed.
                int i = e.Message.Length;
                throw;
            }
        }

        internal static byte[] ExtractData(this byte[] from, int startIndex, int length)
        {
            byte[] localBuffer = new byte[length];
            Buffer.BlockCopy(from, startIndex, localBuffer, 0, length);
            return localBuffer;
        }

        // TODO : Include encoding in parameters.
        internal static string ExtractString(this byte[] from, int startIndex, int length)
        {
            return Encoding.ASCII.GetString(ExtractData(from, startIndex, length));
        }

        internal static byte[] FromUInt16(this ushort data, Endianness endianness)
        {
            byte[] result = new byte[sizeof(ushort)];
            switch (endianness) {
                case Endianness.BigEndian:
                    for(int index = sizeof(ushort); index >= 0;) {
                        result[--index] = (byte)(data % 256);
                        data /= 256;
                    }
                    break;
                case Endianness.LittleEndian:
                    for(int index = 0; index < sizeof(ushort); index++) {
                        result[index] = (byte)(data % 256);
                        data /= 256;
                    }
                    break;
                default:
                    throw new ApplicationException();
            }
            return result;
        }

        internal static ushort ToUInt16(this byte[] data, Endianness endianness, int offset)
        {
            ushort result = 0;
            switch (endianness) {
                case Endianness.BigEndian:
                    for (int index = 0; index < sizeof(ushort); index++) {
                        result <<= 8;
                        result += data[offset++];
                    }
                    break;
                case Endianness.LittleEndian:
                    for (int index = 0; index < sizeof(ushort); index++) {
                        result += (ushort)(data[offset++] << (8 * index));
                    }
                    break;
                default:
                    throw new ApplicationException();
            }
            return result;
        }
    }
}
