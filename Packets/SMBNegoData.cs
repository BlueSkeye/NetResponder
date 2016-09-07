using System;
using System.Collections.Generic;

namespace NetResponder.Packets
{
    internal class SMBNegoData : BasePacket
    {
        static SMBNegoData()
        {
            List<byte> builder = new List<byte>();
            Append(builder, out _wordcountDescriptor, new byte[] { 0x00 });
            Append(builder, out _bccDescriptor, new byte[] { 0x54, 0x00 });
            Append(builder, out _separator1Descriptor, new byte[] { 0x02 });
            Append(builder, out _dialect1Descriptor, new byte[] { 0x50, 0x43, 0x20, 0x4e, 0x45, 0x54, 0x57, 0x4f, 0x52, 0x4b, 0x20, 0x50, 0x52, 0x4f, 0x47, 0x52, 0x41, 0x4d, 0x20, 0x31, 0x2e, 0x30, 0x00 });
            Append(builder, out _separator2Descriptor, new byte[] { 0x02 });
            Append(builder, out _dialect2Descriptor, new byte[] { 0x4c, 0x41, 0x4e, 0x4d, 0x41, 0x4e, 0x31, 0x2e, 0x30, 0x00 });
            _defaultPacket = builder.ToArray();
        }

        internal SMBNegoData()
            : base(_defaultPacket)
        {
            return;
        }

        internal void Calculate()
        {
            throw new NotImplementedException();
            //CalculateBCC = str(self.fields["separator1"]) + str(self.fields["dialect1"])
            //CalculateBCC += str(self.fields["separator2"]) + str(self.fields["dialect2"])
            //self.fields["bcc"] = struct.pack("<h", len(CalculateBCC))
        }

        private byte[] _data;
        private static readonly byte[] _defaultPacket;
        private static ItemDescriptor _wordcountDescriptor;
        private static ItemDescriptor _bccDescriptor;
        private static ItemDescriptor _separator1Descriptor;
        private static ItemDescriptor _dialect1Descriptor;
        private static ItemDescriptor _separator2Descriptor;
        private static ItemDescriptor _dialect2Descriptor;
    }
}
