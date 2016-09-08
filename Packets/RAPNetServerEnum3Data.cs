using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetResponder.Packets
{
    internal class RAPNetServerEnum3Data : BasePacket
    {
        static RAPNetServerEnum3Data()
        {
            List<byte> builder = new List<byte>();
            Append(builder, out  _commandDescriptor, new byte[] { 0xd7, 0x00});
            Append(builder, out  _paramDescriptor, Encoding.ASCII.GetBytes("WrLehDzz"));
            Append(builder, out  _paramDescriptorTerminatorDescriptor, new byte[] { 0x00 });
            Append(builder, out  _returnDescriptor, Encoding.ASCII.GetBytes("B16BBDz"));
            Append(builder, out  _returnDescriptorTerminatorDescriptor, new byte[] { 0x00 });
            Append(builder, out  _detailLevelDescriptor, new byte[] { 0x01, 0x00 });
            Append(builder, out  _recvBuffDescriptor, new byte[] { 0xff, 0xff });
            Append(builder, out  _serverTypeDescriptor, new byte[] { 0x00, 0x00, 0x00, 0x80 });
            Append(builder, out  _targetDomainDescriptor, Encoding.ASCII.GetBytes("SMB"));
            Append(builder, out  _rapTerminatorDescriptor, new byte[] { 0x00 });
            Append(builder, out  _targetNameDescriptor, Encoding.ASCII.GetBytes("ABCD"));
            Append(builder, out  _rapTerminator2Descriptor, new byte[] { 0x00 });
            _defaultPacket = builder.ToArray();
        }

        internal RAPNetServerEnum3Data()
            : base(_defaultPacket)
        {
            return;
        }

        internal byte[] DetailLevel
        {
            get { return GetData(_detailLevelDescriptor); }
            set { SetData(_detailLevelDescriptor, value); }
        }

        internal byte[] ServerType
        {
            get { return GetData(_serverTypeDescriptor); }
            set { SetData(_serverTypeDescriptor, value); }
        }

        internal byte[] TargetDomain
        {
            get { return GetData(_targetDomainDescriptor); }
            set { SetData(_targetDomainDescriptor, value); }
        }

        private static readonly byte[] _defaultPacket;
        private static ItemDescriptor _commandDescriptor;
        private static ItemDescriptor _paramDescriptor;
        private static ItemDescriptor _paramDescriptorTerminatorDescriptor;
        private static ItemDescriptor _returnDescriptor;
        private static ItemDescriptor _returnDescriptorTerminatorDescriptor;
        private static ItemDescriptor _detailLevelDescriptor;
        private static ItemDescriptor _recvBuffDescriptor;
        private static ItemDescriptor _serverTypeDescriptor;
        private static ItemDescriptor _targetDomainDescriptor;
        private static ItemDescriptor _rapTerminatorDescriptor;
        private static ItemDescriptor _targetNameDescriptor;
        private static ItemDescriptor _rapTerminator2Descriptor;
    }
}
