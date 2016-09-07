using System;
using System.Text;

namespace NetResponder
{
    internal static class Extension
    {
        internal static bool Equals(this byte[] x, byte[] y)
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

        internal static byte[] FromUInt16(this ushort data)
        {
            // This operation is intended to be performed on a program variable
            // or property. The result will be big endian encoded.
            // TODO : Be more explicit as of BE/LE
            byte[] result = new byte[sizeof(ushort)];
            for(int index = sizeof(ushort); index >= 0;) {
                result[--index] = (byte)(data % 256);
                data /= 256;
            }
            return result;
        }

        internal static ushort ToUInt16(this byte[] data, ref int offset)
        {
            // This operation is intended to be performed on incoming packets.
            // Numbers are big endian encoded.
            // TODO : Be more explicit as of BE/LE
            ushort result = 0;
            for (int index = 0; index < sizeof(ushort); index++) {
                result <<= 8;
                result += data[offset++];
            }
            return result;
        }
    }
}
