using System;
using System.Collections.Generic;
using System.Text;

namespace NetResponder.Packets
{
    internal class SMBSessionData : BasePacket
    {
        static SMBSessionData()
        {
            List<byte> builder = new List<byte>();
            Append(builder, out _wordcountDescriptor, new byte[] { 0x0a });
            Append(builder, out _andXCommandDescriptor, new byte[] { 0xff });
            Append(builder, out _reservedDescriptor, new byte[] { 0x00 });
            Append(builder, out _andxoffsetDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _maxbuffDescriptor, new byte[] { 0xff, 0xff });
            Append(builder, out _maxmpxDescriptor, new byte[] { 0x02, 0x00 });
            Append(builder, out _vcnumDescriptor, new byte[] { 0x01, 0x00 });
            Append(builder, out _sessionkeyDescriptor, new byte[] { 0x00, 0x00, 0x00, 0x00 });
            Append(builder, out _passwordLenDescriptor, new byte[] { 0x18, 0x00 });
            Append(builder, out _reserved2Descriptor, new byte[] { 0x00,0x00, 0x00, 0x00 });
            Append(builder, out _bccDescriptor, new byte[] { 0x3b, 0x00 });
            Append(builder, out _accountPasswordDescriptor, new byte[0]);
            Append(builder, out _accountNameDescriptor, new byte[0]);
            Append(builder, out _accountNameTerminatorDescriptor, new byte[] { 0x00 });
            Append(builder, out _primaryDomainDescriptor, Encoding.ASCII.GetBytes("WORKGROUP"));
            Append(builder, out _primaryDomainTerminatorDescriptor, new byte[] { 0x00 });
            Append(builder, out _nativeOsDescriptor, Encoding.ASCII.GetBytes("Unix"));
            Append(builder, out _nativeOsTerminatorDescriptor, new byte[] { 0x00 });
            Append(builder, out _nativeLanmanDescriptor, Encoding.ASCII.GetBytes("Samba"));
            Append(builder, out _nativeLanmanTerminatorDescriptor, new byte[] { 0x00 });
            _defaultPacket = builder.ToArray();
        }

        internal SMBSessionData()
            : base(_defaultPacket)
        {
            return;
        }

        internal void Calculate()
        {
            List<byte> bccData = new List<byte>();
            bccData.AddRange(AccountPassword);
            bccData.AddRange(AccountName);
            bccData.AddRange(AccountNameTerminator);
            bccData.AddRange(PrimaryDomain);
            bccData.AddRange(PrimaryDomainTerminator);
            bccData.AddRange(NativeOs);
            bccData.AddRange(NativeOsTerminator);
            bccData.AddRange(NativeLanman);
            bccData.AddRange(NativeLanmanTerminator);;
            Bcc = ((ushort)bccData.Count).FromUInt16();
            PasswordLen = ((ushort)AccountPassword.Length).FromUInt16();
            return;
        }

        internal byte[] AccountPassword
        {
            get { return GetData(_accountPasswordDescriptor); }
            set { SetData(_accountPasswordDescriptor, value); }
        }

        internal byte[] AccountName
        {
            get { return GetData(_accountNameDescriptor); }
            set { SetData(_accountNameDescriptor, value); }
        }

        internal byte[] AccountNameTerminator
        {
            get { return GetData(_accountNameTerminatorDescriptor); }
            set { SetData(_accountNameTerminatorDescriptor, value); }
        }

        internal byte[] Bcc
        {
            get { return GetData(_bccDescriptor); }
            set { SetData(_bccDescriptor, value); }
        }

        internal byte[] NativeLanman
        {
            get { return GetData(_nativeLanmanDescriptor); }
            set { SetData(_nativeLanmanDescriptor, value); }
        }

        internal byte[] NativeLanmanTerminator
        {
            get { return GetData(_nativeLanmanTerminatorDescriptor); }
            set { SetData(_nativeLanmanTerminatorDescriptor, value); }
        }

        internal byte[] NativeOs
        {
            get { return GetData(_nativeOsDescriptor); }
            set { SetData(_nativeOsDescriptor, value); }
        }

        internal byte[] NativeOsTerminator
        {
            get { return GetData(_nativeOsTerminatorDescriptor); }
            set { SetData(_nativeOsTerminatorDescriptor, value); }
        }

        internal byte[] PasswordLen
        {
            get { return GetData(_passwordLenDescriptor); }
            set { SetData(_passwordLenDescriptor, value); }
        }

        internal byte[] PrimaryDomain
        {
            get { return GetData(_primaryDomainDescriptor); }
            set { SetData(_primaryDomainDescriptor, value); }
        }

        internal byte[] PrimaryDomainTerminator
        {
            get { return GetData(_primaryDomainTerminatorDescriptor); }
            set { SetData(_primaryDomainTerminatorDescriptor, value); }
        }

        private static readonly byte[] _defaultPacket;
        private static ItemDescriptor _wordcountDescriptor;
        private static ItemDescriptor _andXCommandDescriptor;
        private static ItemDescriptor _reservedDescriptor;
        private static ItemDescriptor _andxoffsetDescriptor;
        private static ItemDescriptor _maxbuffDescriptor;
        private static ItemDescriptor _maxmpxDescriptor;
        private static ItemDescriptor _vcnumDescriptor;
        private static ItemDescriptor _sessionkeyDescriptor;
        private static ItemDescriptor _passwordLenDescriptor;
        private static ItemDescriptor _reserved2Descriptor;
        private static ItemDescriptor _bccDescriptor;
        private static ItemDescriptor _accountPasswordDescriptor;
        private static ItemDescriptor _accountNameDescriptor;
        private static ItemDescriptor _accountNameTerminatorDescriptor;
        private static ItemDescriptor _primaryDomainDescriptor;
        private static ItemDescriptor _primaryDomainTerminatorDescriptor;
        private static ItemDescriptor _nativeOsDescriptor;
        private static ItemDescriptor _nativeOsTerminatorDescriptor;
        private static ItemDescriptor _nativeLanmanDescriptor;
        private static ItemDescriptor _nativeLanmanTerminatorDescriptor;
    }
}
