using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetResponder.Packets
{
    /// <summary>As per RFC1002 - 4.4.1</summary>
    internal class NetBIOSUdpHeader : BasePacket
    {
        static NetBIOSUdpHeader()
        {
            List<byte> builder = new List<byte>();
            Append(builder, out _messageTypeDescriptor, new byte[] { 0x00 });
            Append(builder, out _flagsDescriptor, new byte[] { 0x00 });
            Append(builder, out _datagramIdDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _sourceIpDescriptor, new byte[] { 0x00, 0x00, 0x00, 0x00 });
            Append(builder, out _sourcePortDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _datagramLengthDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _packetOffsetDescriptor, new byte[] { 0x00, 0x00 });
            _defaultPacket = builder.ToArray();
        }

        internal NetBIOSUdpHeader()
            : base(_defaultPacket)
        {
            return;
        }

        private NetBIOSUdpHeader(byte[] responseData, bool response)
            : base(responseData)
        {
            _responseHeader = response;
            return;
        }

        internal static NetBIOSUdpHeader FromRequest(byte[] data)
        {
            // Don't forget to skip length prefix.
            byte[] headerData = data.ExtractData(_defaultPacket.Length);
            return new NetBIOSUdpHeader(headerData, false);
        }

        private bool _responseHeader;
        private static readonly byte[] _defaultPacket;
        private static ItemDescriptor _messageTypeDescriptor;
        private static ItemDescriptor _flagsDescriptor;
        private static ItemDescriptor _datagramIdDescriptor;
        private static ItemDescriptor _sourceIpDescriptor;
        private static ItemDescriptor _sourcePortDescriptor;
        private static ItemDescriptor _datagramLengthDescriptor;
        private static ItemDescriptor _packetOffsetDescriptor;
    }
}
