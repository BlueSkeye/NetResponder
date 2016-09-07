using System;
using System.Text;

namespace NetResponder
{
    internal static class Extension
    {
        internal static byte[] ExtractData(this byte[] from, int startIndex)
        {
            return from.ExtractData(startIndex, from.Length - startIndex);
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

        internal static ushort ReadUInt16(this byte[] data, ref int offset)
        {
            // This operation is intended to be performed on incoming packets.
            // Numbers are big endian encoded.
            // TODO : Be more explicit as of BE/LE
            ushort result = 0;
            for(int index = 0; index < sizeof(ushort); index++) {
                result <<= 8;
                result += data[offset++];
            }
            return result;
        }
    }
}
