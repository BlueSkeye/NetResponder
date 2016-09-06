using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace NetResponder.Servers
{
    /// <summary>Subclasses must implement the 20.17.3 Request Handler objects model
    /// as defined in :
    /// https://docs.python.org/2/library/socketserver.html#SocketServer.BaseServer.RequestHandlerClass
    /// The setup/finish is optional for performance considetations.
    /// </summary>
    internal abstract class UDPServer
    {
        protected UDPServer(ushort port)
        {
            Port = port;
        }

        protected ushort Port { get; private set; }

        private void DoReceiveLoop()
        {
            throw new NotImplementedException();
        }

        protected abstract void HandleIncomingData(byte[] request);

        internal void Start()
        {
            try {
                this.DoReceiveLoop();
            }
            catch {
                Helpers.PrintError(
                    "[!] Error starting UDP server on port {0}, check permissions or other servers running.",
                    Port);
            }
        }
    }
}
