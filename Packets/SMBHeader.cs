using System;
using System.Collections.Generic;

namespace NetResponder.Packets
{
    internal class SMBHeader : BasePacket
    {
        static SMBHeader()
        {
            List<byte> builder = new List<byte>();
            Append(builder, out _protoDescriptor, new byte[] { 0xff, 0x53, 0x4d, 0x42 });
            Append(builder, out _cmdDescriptor, new byte[] { 0x72 });
            Append(builder, out _errorcodeDescriptor, new byte[] { 0x00, 0x00, 0x00, 0x00 });
            Append(builder, out _flag1Descriptor, new byte[] { 0x00 });
            Append(builder, out _flag2Descriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _pidhighDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _signatureDescriptor, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            Append(builder, out _reservedDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _tidDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _pidDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _uidDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _midDescriptor, new byte[] { 0x00, 0x00 });
            _defaultPacket = builder.ToArray();
        }

        internal SMBHeader()
            : base(_defaultPacket)
        {
            return;
        }

        internal byte[] Cmd
        {
            get { return GetData(_cmdDescriptor); }
            set { SetData(_cmdDescriptor, value); }
        }

        internal byte[] Flag1
        {
            get { return GetData(_flag1Descriptor); }
            set { SetData(_flag1Descriptor, value); }
        }

        internal byte[] Flag2
        {
            get { return GetData(_flag2Descriptor); }
            set { SetData(_flag2Descriptor, value); }
        }

        internal byte[] Mid
        {
            get { return GetData(_midDescriptor); }
            set { SetData(_midDescriptor, value); }
        }

        internal byte[] Pid
        {
            get { return GetData(_pidDescriptor); }
            set { SetData(_pidDescriptor, value); }
        }

        internal byte[] Protocol
        {
            get { return GetData(_protoDescriptor); }
            set { SetData(_protoDescriptor, value); }
        }

        internal byte[] Tid
        {
            get { return GetData(_tidDescriptor); }
            set { SetData(_tidDescriptor, value); }
        }

        internal byte[] Uid
        {
            get { return GetData(_uidDescriptor); }
            set { SetData(_uidDescriptor, value); }
        }

        private static readonly byte[] _defaultPacket;
        private static ItemDescriptor _protoDescriptor;
        private static ItemDescriptor _cmdDescriptor;
        private static ItemDescriptor _errorcodeDescriptor;
        private static ItemDescriptor _flag1Descriptor;
        private static ItemDescriptor _flag2Descriptor;
        private static ItemDescriptor _pidhighDescriptor;
        private static ItemDescriptor _signatureDescriptor;
        private static ItemDescriptor _reservedDescriptor;
        private static ItemDescriptor _tidDescriptor;
        private static ItemDescriptor _pidDescriptor;
        private static ItemDescriptor _uidDescriptor;
        private static ItemDescriptor _midDescriptor;
    }
}