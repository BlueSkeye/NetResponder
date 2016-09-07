using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Text;
using System.Threading;
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
        protected UDPServer(IPEndPoint serverAddress)
        {
            this.server_address = serverAddress;
            this.__is_shut_down = new AutoResetEvent(false);
            this.__shutdown_request = false;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //        if bind_and_activate:
            this._socket.Bind(this.server_address);
            this.server_address = (IPEndPoint)this._socket.LocalEndPoint;
            _readSelectable = new List<Socket>();
        }

        protected ushort Port { get; private set; }

        private void _handle_request_noblock()
        {
            byte[] data = new byte[this.max_packet_size];
            EndPoint client_addr = new IPEndPoint(IPAddress.Any, 0);
            int receiveCount;
            try { receiveCount = this._socket.ReceiveFrom(data, ref client_addr); }
            catch (SocketException e) { return; }
            try { HandleIncomingData((IPEndPoint)client_addr, data); }
            catch (Exception e) {
                Console.WriteLine(SeparatorLine);
                Console.WriteLine("Exception happened during processing of request from {0}", client_addr);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(SeparatorLine);
            }
        }

        internal void Close()
        {
            _socket.Close();
        }

        private void DoReceiveLoop()
        {
            this.__is_shut_down.Reset();
            try {
                while (!this.__shutdown_request) {
                    //# XXX: Consider using another file descriptor or
                    //# connecting to the socket to wake this up instead of
                    //# polling. Polling reduces our responsiveness to a
                    //# shutdown request and wastes cpu at all other times.
                    _readSelectable.Add(_socket);
                    Socket.Select(_readSelectable, null, null, SelectionDelay);
                    //r, w, e = select.select([self], [], [], poll_interval)
                    if (_readSelectable.Contains(_socket)) {
                        this._handle_request_noblock();
                    }
                }
            }
            finally {
                this.__shutdown_request = false;
                this.__is_shut_down.Set();
            }
            throw new NotImplementedException();
        }

        protected abstract void HandleIncomingData(IPEndPoint from, byte[] request);

        internal void Start()
        {
            try { this.DoReceiveLoop(); }
            catch {
                Helpers.PrintError(
                    "[!] Error starting UDP server on port {0}, check permissions or other servers running.",
                    Port);
            }
        }

        internal void Shutdown()
        {
            __shutdown_request = true;
            __is_shut_down.WaitOne();
        }

        private static readonly int SelectionDelay = (int)(TimeSpan.FromMilliseconds(500).TotalMilliseconds * 1000);
        private static readonly string SeparatorLine = new string('-', 40);
        private bool allow_reuse_address = true;
        private int max_packet_size = 8192;
        private IPEndPoint server_address;
        private AutoResetEvent __is_shut_down;
        private List<Socket> _readSelectable;
        private bool __shutdown_request;
        private Socket _socket;
    }
}
