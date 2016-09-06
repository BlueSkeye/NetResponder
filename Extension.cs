using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetResponder
{
    internal static class Extension
    {
        // TODO : Include encoding in parameters.
        internal static string ExtractString(this byte[] from, int startIndex, int length)
        {
            byte[] localBuffer = new byte[length];
            Buffer.BlockCopy(from, startIndex, localBuffer, 0, length);
            return Encoding.ASCII.GetString(localBuffer);
        } 
    }
}
