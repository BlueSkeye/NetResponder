using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using NetResponder.Packets;
using NetResponder.Protocols;

namespace NetResponder.Servers
{
    internal class BrowsingService : UDPServer
    {
        /// <summary></summary>
        /// <param name="port"></param>
        /// <param name="analyzeMode">We capture analyze mode setting at startup.
        /// This is different from original implementation and doesn't allow for
        /// "on the fly" modification of the attribute.</param>
        internal BrowsingService(IPEndPoint address, bool analyzeMode)
            : base(address)
        {
            _analyze = analyzeMode;
            return;
        }

        private string DecodeRequestType(byte type)
        {
            switch (type) {
                case 0x01: return "Host Announcement";
                case 0x02: return "Request Announcement";
                case 0x08: return "Browser Election";
                case 0x09: return "Get Backup List Request";
                case 0x0a: return "Get Backup List Response";
                case 0x0b: return "Become Backup Browser";
                case 0x0c: return "Domain/Workgroup Announcement";
                case 0x0d: return "Master Announcement";
                case 0x0e: return "Reset Browser State Announcement";
                case 0x0f: return "Local Master Announcement";
                default: return "Unknown";
            }
        }

        protected override void HandleIncomingData(IPEndPoint from, byte[] request)
        {
            try {
                if (_analyze) {
                    ParseDatagramNBTNames(from, request);
                }
                BecomeBackup(from, request);
            }
            catch { }
        }

        private void ParseDatagramNBTNames(IPEndPoint from, byte[] data)
        {
            // TODO : Check name encoding and assert the ExtractString is inline
            // with it.
            string domain = NetBIOSHelpers.DecodeName(
                data.ExtractString(49, NetBIOSHelpers.NetbiosNameLength));
            string name = NetBIOSHelpers.DecodeName(
                data.ExtractString(15, NetBIOSHelpers.NetbiosNameLength));
            string role1 = NetBIOSHelpers.GetRoleById(
                data.ExtractData(45, NetBIOSHelpers.NetbiosRoleIdLength));
            string role2 = NetBIOSHelpers.GetRoleById(
                data.ExtractData(79, NetBIOSHelpers.NetbiosRoleIdLength));
            if (_analyze) {
                switch (role2) {
                    case "Domain Controller":
                    case "Browser Election":
                    case "Local Master Browser":
                        Console.WriteLine(
                            "[Analyze mode: Browser] Datagram Request from IP: {0} hostname: {1} via the: {2} to: {3}. Service: {4}",
                            from, name, role1, domain, role2);
                        RAPThisDomain(from, domain);
                        break;
                    default:
                        break;
                }
            }
        }

        private void RAPThisDomain(IPEndPoint from, string domain)
        {
            //    PDC = RapFinger(from, domain, new byte[] { 0x00, 0x00, 0x00, 0x80});
            //	if PDC is not None:
            //        print text("[LANMAN] Detected Domains: %s" % ', '.join(PDC))

            //	SQL = RapFinger(Client, Domain,"\x04\x00\x00\x00")
            //	if SQL is not None:
            //        print text("[LANMAN] Detected SQL Servers on domain %s: %s" % (Domain, ', '.join(SQL)))

            //	WKST = RapFinger(Client, Domain,"\xff\xff\xff\xff")
            //	if WKST is not None:
            //        print text("[LANMAN] Detected Workstations/Servers on domain %s: %s" % (Domain, ', '.join(WKST)))
            throw new NotImplementedException();
        }

        //def WorkstationFingerPrint(data):
        //	return {
        //		"\x04\x00"    :"Windows 95",
        //		"\x04\x10"    :"Windows 98",
        //		"\x04\x90"    :"Windows ME",
        //		"\x05\x00"    :"Windows 2000",
        //		"\x05\x01"    :"Windows XP",
        //		"\x05\x02"    :"Windows XP(64-Bit)/Windows 2003",
        //		"\x06\x00"    :"Windows Vista/Server 2008",
        //		"\x06\x01"    :"Windows 7/Server 2008R2",
        //		"\x06\x02"    :"Windows 8/Server 2012",
        //		"\x06\x03"    :"Windows 8.1/Server 2012R2",
        //		"\x10\x00"    :"Windows 10/Server 2016",
        //	}.get(data, 'Unknown')


        //def PrintServerName(data, entries):
        //	if entries <= 0:
        //		return None
        //    entrieslen = 26 * entries

        //    chunks, chunk_size = len(data[:entrieslen]), entrieslen/entries
        //    ServerName = [data[i: i + chunk_size] for i in range(0, chunks, chunk_size)]


        //    l = []
        //	for x in ServerName:
        //		fingerprint = WorkstationFingerPrint(x[16:18])

        //        name = x[:16].replace('\x00', '')

        //        l.append('%s (%s)' % (name, fingerprint))
        //	return l


        //def ParsePacket(Payload):

        //    PayloadOffset = struct.unpack('<H', Payload[51:53])[0]
        //        StatusCode = Payload[PayloadOffset - 4:PayloadOffset - 2]

        //	if StatusCode == "\x00\x00":
        //		EntriesNum = struct.unpack('<H', Payload[PayloadOffset:PayloadOffset + 2])[0]
        //		return PrintServerName(Payload[PayloadOffset + 4:], EntriesNum)
        //	return None


        private void RapFinger(IPEndPoint from, string domain, byte[] data)
        {
            //def RapFinger(Host, Domain, Type):
            try {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Connect(from.Address, 445);
                s.ReceiveTimeout = s.SendTimeout = 300;
                SMBHeader Header = new SMBHeader() {
                    Cmd = new byte[] { 0x72 },
                    Mid = new byte[] { 0x01, 0x00 } };
                SMBNegoData Body = new SMBNegoData();
                Body.Calculate();
                byte[] packet = BasePacket.Concatenate(sizeof(int), Header, Body);
                packet.Write(0, true, (packet.Length - sizeof(int)));
                // byte[] buffer = struct.pack(">i", len(''.join(Packet))) + Packet;
                s.Send(packet);
                data = new byte[1024];
                int receivedCount = s.Receive(data);

                // # Session Setup AndX Request, Anonymous.
                //if (data[8:10] != "\x72\x00") { return; }
                //Header = new SMBHeader() {
                //    Cmd = new byte[] { 0x73 },
                //    Mid = new byte[] { 0x02, 0x00 } };
                //Body = new SMBSessionData();
                //Body.Calculate();
                //packet = BasePacket.Concatenate(sizeof(int), Header, Body);
                //packet.Write(0, true, (packet.Length - sizeof(int)));
                //// Buffer = struct.pack(">i", len(''.join(Packet))) + Packet
                //s.Send(packet);
                //receivedCount = s.Receive(data);

                // # Tree Connect IPC$.
                //if (data[8:10] != "\x73\x00") { return; }
                //Header = new SMBHeader() {
                //    Cmd = new byte[] { 0x75 },
                //    Flag1 = new byte[] { 0x08 },
                //    Flag2 = new byte[] { 0x01, 0x00 },
                //    Uid = data[32:34],
                //    Mid = new byte[] { 0x03, 0x00 } };
                //Body = new SMBTreeConnectData() {
                //    Path = "\\\\" + Host + "\\IPC$"
                //};
                //Body.Calculate();
                //packet = BasePacket.Concatenate(sizeof(int), Header, Body);
                //// Buffer = struct.pack(">i", len(''.join(Packet))) + Packet
                //packet.Write(0, true, (packet.Length - sizeof(int)));
                //s.Send(packet);
                //receivedCount = s.Receive(data);

                //				# Rap ServerEnum.
                //				if data[8:10] == "\x75\x00":
                //					Header = SMBHeader(cmd= "\x25", flag1= "\x08", flag2= "\x01\xc8", uid= data[32:34], tid= data[28:30], pid= data[30:32], mid= "\x04\x00")
                //                    Body = SMBTransRAPData(Data= RAPNetServerEnum3Data(ServerType = Type, DetailLevel = "\x01\x00", TargetDomain = Domain))
                //                    Body.calculate()
                //					Packet = str(Header)+str(Body)
                //                    Buffer = struct.pack(">i", len(''.join(Packet))) + Packet
                //                    s.send(Buffer)
                //                    data = s.recv(64736)

                //					# Rap ServerEnum, Get answer and return what we're looking for.
                //					if data[8:10] == "\x25\x00":
                //						s.close()
                //						return ParsePacket(data)
            }
            catch {
                return;
            }
        }

        private void BecomeBackup(IPEndPoint from, byte[] data)
        {
            try {
                int offset = 139;
                //DataOffset    = struct.unpack('<H', data[139:141])[0]
                int dataOffset = data.ReadUInt16(ref offset);
                //BrowserPacket = data[82 + DataOffset:]
                byte[] browserPacket = data.ExtractData(82 + dataOffset);
                string reqType = DecodeRequestType(browserPacket[0]);

                if (reqType != "Become Backup Browser") { return; }
                string serverName = Encoding.ASCII.GetString(browserPacket.ExtractData(1));
                string domain = NetBIOSHelpers.DecodeName(
                    data.ExtractString(49, NetBIOSHelpers.NetbiosNameLength));
                string name = NetBIOSHelpers.DecodeName(
                    data.ExtractString(15, NetBIOSHelpers.NetbiosNameLength));
                string role = NetBIOSHelpers.GetRoleById(
                    data.ExtractData(45, NetBIOSHelpers.NetbiosRoleIdLength));
                if (_analyze) {
                    Console.WriteLine("[Analyze mode: Browser] Datagram Request from IP: {0} hostname: {1} via the: {2} wants to become a Local Master Browser Backup on this domain: {3}.",
                        from, name, role, domain);
                    //print RAPThisDomain(Client, Domain)
                }
            }
            catch (Exception e) { return; }
        }

        private bool _analyze;
    }
}
