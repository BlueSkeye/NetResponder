using System;
using System.Collections.Generic;
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

        private string[] ParsePacket(byte[] payload)
        {
            ushort payloadOffset = payload.ToUInt16(Endianness.LittleEndian, 51);
            ushort statusCode = payload.ToUInt16(Endianness.LittleEndian, (payloadOffset - 4));
            if (0 == statusCode) {
                ushort entriesCount = payload.ToUInt16(Endianness.LittleEndian, payloadOffset);
                return PrintServerName(payload.ExtractData(payloadOffset + 4), entriesCount);
            }
            return null;
        }

        private string[] PrintServerName(byte[] data, int entries)
        {
            if (0 >= entries) { return null; }
            int chunk_size = 26;
            int entrieslen = chunk_size * entries;
            //    chunks= len(data[:entrieslen])
            List<byte[]> ServerName = new List<byte[]>();
            List<string> l = new List<string>();
            for (int index = 0; index < entries; index++) {
                byte[] thisChunk = data.ExtractData((index * chunk_size), chunk_size);
                string fingerprint = WorkstationFingerPrint(
                    thisChunk.ToUInt16(Endianness.LittleEndian, 16));
                string name = Encoding.ASCII.GetString(thisChunk.ExtractData(0, 16))
                    .Replace("\0", "");
                l.Add(string.Format("{0} ({1})", name, fingerprint));
            }
            return l.ToArray();
        }

        private string[] RapFinger(IPEndPoint from, string domain, byte[] type)
        {
            try {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Connect(from.Address, 445);
                s.ReceiveTimeout = s.SendTimeout = 300;
                SMBHeader header = new SMBHeader() {
                    Cmd = new byte[] { 0x72 },
                    Mid = new byte[] { 0x01, 0x00 } };
                SMBNegoData body = new SMBNegoData();
                body.Calculate();
                byte[] packet = BasePacket.Concatenate(sizeof(int), header, body);
                packet.Write(0, true, (packet.Length - sizeof(int)));
                // byte[] buffer = struct.pack(">i", len(''.join(Packet))) + Packet;
                int sentLength = s.Send(packet);
                byte[] data = new byte[1024];
                int receivedCount = s.Receive(data);

                // # Session Setup AndX Request, Anonymous.
                if (!data.ExtractData(8, 2).ContentEquals(new byte[] { 0x72, 0x00 })) { return null; }
                header = new SMBHeader() {
                    Cmd = new byte[] { 0x73 },
                    Mid = new byte[] { 0x02, 0x00 } };
                SMBSessionData sessionDatabody = new SMBSessionData();
                body.Calculate();
                packet = BasePacket.Concatenate(sizeof(int), header, sessionDatabody);
                packet.Write(0, true, (packet.Length - sizeof(int)));
                //// Buffer = struct.pack(">i", len(''.join(Packet))) + Packet
                sentLength = s.Send(packet);
                receivedCount = s.Receive(data);

                //# Tree Connect IPC$.
                if (!data.ExtractData(8, 2).ContentEquals(new byte[] { 0x73, 0x00 })) { return null; }
                header = new SMBHeader() {
                    Cmd = new byte[] { 0x75 },
                    Flag1 = new byte[] { 0x08 },
                    Flag2 = new byte[] { 0x01, 0x00 },
                    Uid = data.ExtractData(32, 2),
                    Mid = new byte[] { 0x03, 0x00 } };
                SMBTreeConnectData treeConnectBody = new SMBTreeConnectData() {
                    Path = Encoding.ASCII.GetBytes("\\\\" + from.Address.ToString() + "\\IPC$")
                };
                treeConnectBody.Calculate();
                packet = BasePacket.Concatenate(sizeof(int), header, treeConnectBody);
                packet.Write(0, true, (packet.Length - sizeof(int)));
                sentLength = s.Send(packet);
                receivedCount = s.Receive(data);

                // # Rap ServerEnum.
                if (!data.ExtractData(8, 2).ContentEquals(new byte[] { 0x75, 0x00 })) { return null; }
                header = new SMBHeader() {
                    Cmd = new byte[] { 0x25 },
                    Flag1 = new byte[] { 0x08 },
                    Flag2 = new byte[] { 0x01, 0xc8 },
                    Uid = data.ExtractData(32, 2),
                    Tid = data.ExtractData(28, 2),
                    Pid = data.ExtractData(30, 2),
                    Mid = new byte[] { 0x04, 0x00 } };
                SMBTransRAPData transRAPBody = new SMBTransRAPData() {
                    Data = new RAPNetServerEnum3Data() {
                        ServerType = type,
                        DetailLevel = new byte[] { 0x01, 0x00 },
                        TargetDomain = Encoding.ASCII.GetBytes(domain)
                    }.RawData };
                transRAPBody.Calculate();
                packet = BasePacket.Concatenate(sizeof(int), header, transRAPBody);
                packet.Write(0, true, (packet.Length - sizeof(int)));
                sentLength = s.Send(packet);
                data = new byte[64736];
                receivedCount = s.Receive(data);

                // # Rap ServerEnum, Get answer and return what we're looking for.
                if (data.ExtractData(8, 2).ContentEquals(new byte[] { 0x25, 0x00 })) {
                    s.Close();
                }
                return ParsePacket(data);
            }
            catch {
                return null;
            }
        }

        /// <summary>Use Remote Administration Protocol (RAP) for domain fingerprinting.
        /// See : https://msdn.microsoft.com/en-us/library/cc240190.aspx
        /// </summary>
        /// <param name="from"></param>
        /// <param name="domain"></param>
        private void RAPThisDomain(IPEndPoint from, string domain)
        {
            string[] PDC = RapFinger(from, domain, new byte[] { 0x00, 0x00, 0x00, 0x80});
            if (null != PDC) {
                Console.WriteLine("[LANMAN] Detected Domains: {0}", PDC.Combine(", "));
            }
            string[] SQL = RapFinger(from, domain, new byte[] { 0x04, 0x00, 0x00, 0x00 });
            if (null != SQL) {
                Console.WriteLine("[LANMAN] Detected SQL Servers on domain {0} : {1}",
                    domain, SQL.Combine(", "));
            }
            string[] WKST = RapFinger(from, domain, new byte[] { 0xff, 0xff, 0xff, 0xff });
            if (null != WKST) {
                Console.WriteLine("[LANMAN] Detected Workstations/Servers on domain {0} : {1}",
                    domain, WKST.Combine(", "));
            }
            return;
        }

        private string WorkstationFingerPrint(ushort id)
        {
            switch (id) {
                case 0x0004: return "Windows 95";
                case 0x0A04: return "Windows 98"; // *
                case 0x5A04: return "Windows ME"; // *
                case 0x0005: return "Windows 2000";
                case 0x0105: return "Windows XP";
                case 0x0205: return "Windows XP(64-Bit)/Windows 2003";
                case 0x0006: return "Windows Vista/Server 2008";
                case 0x0106: return "Windows 7/Server 2008R2";
                case 0x0206: return "Windows 8/Server 2012";
                case 0x0306: return "Windows 8.1/Server 2012R2";
                case 0x000A: return "Windows 10/Server 2016"; // *
                default: return "Unknown";
            }
        }

        private void BecomeBackup(IPEndPoint from, byte[] data)
        {
            try {
                int dataOffset = data.ToUInt16(Endianness.LittleEndian, 139);
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
                    RAPThisDomain(from, domain);
                }
            }
            catch (Exception e) { return; }
        }

        private bool _analyze;
    }
}
