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

        internal byte[] Bcc
        {
            get { return GetData(_bccDescriptor); }
            set { SetData(_bccDescriptor, value); }
        }

        internal byte[] Dialect1
        {
            get { return GetData(_dialect1Descriptor); }
            set { SetData(_dialect1Descriptor, value); }
        }

        internal byte[] Dialect2
        {
            get { return GetData(_dialect2Descriptor); }
            set { SetData(_dialect2Descriptor, value); }
        }

        internal byte[] Separator1
        {
            get { return GetData(_separator1Descriptor); }
            set { SetData(_separator1Descriptor, value); }
        }

        internal byte[] Separator2
        {
            get { return GetData(_separator2Descriptor); }
            set { SetData(_separator2Descriptor, value); }
        }

        internal byte[] Build(SMBHeader header)
        {
            byte[] result = base.Build(sizeof(int), header, this);
            result.Write(0, true, (result.Length - sizeof(int)));
            return result;
        }

        internal byte[] CalculateAndBuild(SMBHeader header)
        {
            Calculate();
            return Build(header);
        }

        internal void Calculate()
        {
            List<byte> bccData = new List<byte>();
            bccData.AddRange(Separator1);
            bccData.AddRange(Dialect1);
            bccData.AddRange(Separator2);
            bccData.AddRange(Dialect2);
            Bcc = ((ushort)bccData.Count).FromUInt16(Endianness.LittleEndian);
            return;
        }

        private static readonly byte[] _defaultPacket;
        private static ItemDescriptor _wordcountDescriptor;
        private static ItemDescriptor _bccDescriptor;
        private static ItemDescriptor _separator1Descriptor;
        private static ItemDescriptor _dialect1Descriptor;
        private static ItemDescriptor _separator2Descriptor;
        private static ItemDescriptor _dialect2Descriptor;
    }
}
