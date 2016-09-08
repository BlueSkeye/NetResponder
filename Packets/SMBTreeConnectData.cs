using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetResponder.Packets
{
    internal class SMBTreeConnectData : BasePacket
    {
        static SMBTreeConnectData()
        {
            List<byte> builder = new List<byte>();
            Append(builder, out _wordcountDescriptor, new byte[] { 0x04 });
            Append(builder, out _andXCommandDescriptor, new byte[] { 0xff });
            Append(builder, out _reservedDescriptor, new byte[] { 0x00 });
            Append(builder, out _andxoffsetDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _flagsDescriptor, new byte[] { 0x08, 0x00 });
            Append(builder, out _passwdLenDescriptor, new byte[] { 0x01, 0x00 });
            Append(builder, out _bccDescriptor, new byte[] { 0x1b, 0x00 });
            Append(builder, out _passwdDescriptor, new byte[] { 0x00 });
            Append(builder, out _pathDescriptor, new byte[] { 0x00 });
            Append(builder, out _pathTerminatorDescriptor, new byte[] { 0x00 });
            Append(builder, out _serviceDescriptor, Encoding.ASCII.GetBytes("?????"));
            Append(builder, out _terminatorDescriptor, new byte[] { 0x00 });
            _defaultPacket = builder.ToArray();
        }

        internal SMBTreeConnectData()
            : base(_defaultPacket)
        {
            return;
        }

        internal void Calculate()
        {
            PasswdLen = ((ushort)Passwd.Length).FromUInt16(Endianness.LittleEndian);
            List<byte> builder = new List<byte>();
            builder.AddRange(Passwd);
            builder.AddRange(Path);
            builder.AddRange(PathTerminator);
            builder.AddRange(Service);
            builder.AddRange(Terminator);
            Bcc = ((ushort)builder.Count).FromUInt16(Endianness.LittleEndian);
            return;
        }

        internal byte[] Bcc
        {
            get { return GetData(_bccDescriptor); }
            set { SetData(_bccDescriptor, value); }
        }

        internal byte[] Passwd
        {
            get { return GetData(_passwdDescriptor); }
            set { SetData(_passwdDescriptor, value); }
        }

        internal byte[] PasswdLen
        {
            get { return GetData(_passwdLenDescriptor); }
            set { SetData(_passwdLenDescriptor, value); }
        }

        internal byte[] Path
        {
            get { return GetData(_pathDescriptor); }
            set { SetData(_pathDescriptor, value); }
        }

        internal byte[] PathTerminator
        {
            get { return GetData(_pathTerminatorDescriptor); }
            set { SetData(_pathTerminatorDescriptor, value); }
        }

        internal byte[] Service
        {
            get { return GetData(_serviceDescriptor); }
            set { SetData(_serviceDescriptor, value); }
        }

        internal byte[] Terminator
        {
            get { return GetData(_terminatorDescriptor); }
            set { SetData(_terminatorDescriptor, value); }
        }

        private static readonly byte[] _defaultPacket;
        private static ItemDescriptor _wordcountDescriptor;
        private static ItemDescriptor _andXCommandDescriptor;
        private static ItemDescriptor _reservedDescriptor;
        private static ItemDescriptor _andxoffsetDescriptor;
        private static ItemDescriptor _flagsDescriptor;
        private static ItemDescriptor _passwdLenDescriptor;
        private static ItemDescriptor _bccDescriptor;
        private static ItemDescriptor _passwdDescriptor;
        private static ItemDescriptor _pathDescriptor;
        private static ItemDescriptor _pathTerminatorDescriptor;
        private static ItemDescriptor _serviceDescriptor;
        private static ItemDescriptor _terminatorDescriptor;
    }
}
