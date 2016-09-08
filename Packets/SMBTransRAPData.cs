using System;
using System.Collections.Generic;
using System.Text;

namespace NetResponder.Packets
{
    internal class SMBTransRAPData : BasePacket
    {
        static SMBTransRAPData()
        {
            List<byte> builder = new List<byte>();
            Append(builder, out _wordcountDescriptor, new byte[] { 0x0e });
            Append(builder, out _totalParamCountDescriptor, new byte[] { 0x24, 0x00 });
            Append(builder, out _totalDataCountDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _maxParamCountDescriptor, new byte[] { 0x08, 0x00 });
            Append(builder, out _maxDataCountDescriptor, new byte[] { 0xff, 0xff });
            Append(builder, out _maxSetupCountDescriptor, new byte[] { 0x00 });
            Append(builder, out _reservedDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _flagsDescriptor, new byte[] { 0x00 });
            Append(builder, out _timeoutDescriptor, new byte[] { 0x00, 0x00, 0x00, 0x00 });
            Append(builder, out _reserved1Descriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _paramCountDescriptor, new byte[] { 0x24, 0x00 });
            Append(builder, out _paramOffsetDescriptor, new byte[] { 0x5a, 0x00 });
            Append(builder, out _dataCountDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _dataOffsetDescriptor, new byte[] { 0x7e, 0x00 });
            Append(builder, out _setupCountDescriptor, new byte[] { 0x00 });
            Append(builder, out _reserved2Descriptor, new byte[] { 0x00 });
            Append(builder, out _bccDescriptor, new byte[] { 0x3f, 0x00 });
            Append(builder, out _terminatorDescriptor, new byte[] { 0x00 });
            Append(builder, out _pipeNameDescriptor, Encoding.ASCII.GetBytes("\\PIPE\\LANMAN"));
            Append(builder, out _pipeTerminatorDescriptor, new byte[] { 0x00, 0x00 });
            Append(builder, out _dataDescriptor, new byte[0]);
            _defaultPacket = builder.ToArray();
        }

        internal SMBTransRAPData()
            : base(_defaultPacket)
        {
            return;
        }

        internal byte[] Data
        {
            get { return GetData(_dataDescriptor); }
            set { SetData(_dataDescriptor, value); }
        }

        internal void Calculate()
        {
            throw new NotImplementedException();
		    //if len(str(self.fields["Data"]))%2==0:
		    //   self.fields["PipeTerminator"] = "\x00\x00\x00\x00"
		    //else:
		    //   self.fields["PipeTerminator"] = "\x00\x00\x00"
		    //##Convert Path to Unicode first before any Len calc.
		    //self.fields["PipeName"] = self.fields["PipeName"].encode('utf-16le')
		    //##Data Len
		    //self.fields["TotalParamCount"] = struct.pack("<i", len(str(self.fields["Data"])))[:2]
      //      self.fields["ParamCount"] = struct.pack("<i", len(str(self.fields["Data"])))[:2]
      //      ##Packet len
      //      FindRAPOffset = str(self.fields["Wordcount"])+str(self.fields["TotalParamCount"])+str(self.fields["TotalDataCount"])+str(self.fields["MaxParamCount"])+str(self.fields["MaxDataCount"])+str(self.fields["MaxSetupCount"])+str(self.fields["Reserved"])+str(self.fields["Flags"])+str(self.fields["Timeout"])+str(self.fields["Reserved1"])+str(self.fields["ParamCount"])+str(self.fields["ParamOffset"])+str(self.fields["DataCount"])+str(self.fields["DataOffset"])+str(self.fields["SetupCount"])+str(self.fields["Reserved2"])+str(self.fields["Bcc"])+str(self.fields["Terminator"])+str(self.fields["PipeName"])+str(self.fields["PipeTerminator"])
      //      self.fields["ParamOffset"] = struct.pack("<i", len(FindRAPOffset)+32)[:2]
      //      ##Bcc Buff Len
      //      BccComplete    = str(self.fields["Terminator"])+str(self.fields["PipeName"])+str(self.fields["PipeTerminator"])+str(self.fields["Data"])
      //      self.fields["Bcc"] = struct.pack("<i", len(BccComplete))[:2]
        }

        private static readonly byte[] _defaultPacket;
        private static ItemDescriptor _wordcountDescriptor;
        private static ItemDescriptor _totalParamCountDescriptor;
        private static ItemDescriptor _totalDataCountDescriptor;
        private static ItemDescriptor _maxParamCountDescriptor;
        private static ItemDescriptor _maxDataCountDescriptor;
        private static ItemDescriptor _maxSetupCountDescriptor;
        private static ItemDescriptor _reservedDescriptor;
        private static ItemDescriptor _flagsDescriptor;
        private static ItemDescriptor _timeoutDescriptor;
        private static ItemDescriptor _reserved1Descriptor;
        private static ItemDescriptor _paramCountDescriptor;
        private static ItemDescriptor _paramOffsetDescriptor;
        private static ItemDescriptor _dataCountDescriptor;
        private static ItemDescriptor _dataOffsetDescriptor;
        private static ItemDescriptor _setupCountDescriptor;
        private static ItemDescriptor _reserved2Descriptor;
        private static ItemDescriptor _bccDescriptor;
        private static ItemDescriptor _terminatorDescriptor;
        private static ItemDescriptor _pipeNameDescriptor;
        private static ItemDescriptor _pipeTerminatorDescriptor;
        private static ItemDescriptor _dataDescriptor;
    }
}
