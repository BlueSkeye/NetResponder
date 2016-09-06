using System;

namespace NetResponder.Protocols
{
    public class ProtocolException : ApplicationException
    {
        internal ProtocolException(string message)
            : base(message)
        {
            return;
        }

        internal ProtocolException(string message, Exception innerException)
            :base(message, innerException)
        {
            return;
        }
    }
}
