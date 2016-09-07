using System;
using System.Net;
using System.Net.Sockets;

namespace NetResponder
{
    internal static class Helpers
    {
        internal static void DisplayBanner()
        {
            string banner = 
                "                                         __\\\n" +
                "  .----.-----.-----.-----.-----.-----.--|  |.-----.----.\n" +
                "  |   _|  -__|__ --|  _  |  _  |     |  _  ||  -__|   _|\n" +
                "  |__| |_____|_____|   __|_____|__|__|_____||_____|__|\n" +
                "                   |__|";
            Console.Write(banner);
            Console.WriteLine();
            Console.Write("           ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("NBT-NS, LLMNR & MDNS {0}", Constants.Version);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("  Author: Laurent Gaffie (laurent.gaffie@gmail.com)");
            Console.WriteLine("  To kill this script hit CRTL-C");
            Console.WriteLine();
        }

        internal static void DisplayStartupMessage()
        {
            //enabled = color('[ON]', 2, 1)
            //disabled = color('[OFF]', 1, 1)

            Console.WriteLine();
            Helpers.PrintPrefix();
            Console.WriteLine("Poisoners:");
            PrintEnablement(true);
            Console.WriteLine("LLMNR");
            PrintEnablement(true);
            Console.WriteLine("NBT-NS");
            PrintEnablement(true);
            Console.WriteLine("DNS/MDNS");

            Console.WriteLine();
            Helpers.PrintPrefix();
            Console.WriteLine("Servers:");

            PrintEnablement(Settings.Config.HTTPEnabled);
            Console.WriteLine("HTTP server");
            PrintEnablement(Settings.Config.SSLEnabled);
            Console.WriteLine("HTTPS server");
            PrintEnablement(Settings.Config.WPADEnabled);
            Console.WriteLine("WPAD proxy");
            PrintEnablement(Settings.Config.SMBEnabled);
            Console.WriteLine("SMB server");
            PrintEnablement(Settings.Config.KrbEnabled);
            Console.WriteLine("Kerberos server");
            PrintEnablement(Settings.Config.SQLEnabled);
            Console.WriteLine("SQL server");
            PrintEnablement(Settings.Config.FTPEnabled);
            Console.WriteLine("FTP server");
            PrintEnablement(Settings.Config.IMAPEnabled);
            Console.WriteLine("IMAP server");
            PrintEnablement(Settings.Config.POPEnabled);
            Console.WriteLine("POP3 server");
            PrintEnablement(Settings.Config.SMTPEnabled);
            Console.WriteLine("SMTP server");
            PrintEnablement(Settings.Config.DNSEnabled);
            Console.WriteLine("DNS server");
            PrintEnablement(Settings.Config.LDAPEnabled);
            Console.WriteLine("LDAP server");
            Console.WriteLine();

            Helpers.PrintPrefix();
            Console.WriteLine("HTTP Options:");

            PrintEnablement(Settings.Config.ServeAlways);
            Console.WriteLine("Always serving EXE");
            PrintEnablement(Settings.Config.ServeExe);
            Console.WriteLine("Serving EXE");
            PrintEnablement(Settings.Config.ServeHtml);
            Console.WriteLine("Serving HTML");
            PrintEnablement(Settings.Config.UpstreamProxy);
            Console.WriteLine("Upstream Proxy");
            Console.WriteLine("WPAD script");
            Console.WriteLine(Settings.Config.WPADScript);
            Console.WriteLine();

            Helpers.PrintPrefix();
            Console.WriteLine("Poisoning Options:");

            PrintEnablement(Settings.Config.AnalyzeMode);
            Console.WriteLine("Analyze Mode");
            PrintEnablement(Settings.Config.WPADAuthEnabled);
            Console.WriteLine("Force WPAD auth");
            PrintEnablement(Settings.Config.BasicAuthEnabled);
            Console.WriteLine("Force Basic Auth");
            PrintEnablement(Settings.Config.LMEnabled);
            Console.WriteLine("Force LM downgrade");
            PrintEnablement(Settings.Config.FingerEnabled);
            Console.WriteLine("Fingerprint hosts");
            Console.WriteLine();

            Helpers.PrintPrefix();
            Console.WriteLine("Generic Options:");

            Console.Write("    Responder NIC");
            Print(ConsoleColor.Cyan, "[{0}]", Settings.Config.Interface);
            Console.Write("    Responder IP");
            Print(ConsoleColor.Cyan, "[{0}]", Settings.Config.BindTo);
            Console.Write("    Challenge set");
            Print(ConsoleColor.Cyan, "[{0}]", Settings.Config.NumCha1);

            if (Settings.Config.UpstreamProxy) {
                Console.Write("    Upstream Proxy");
                Print(ConsoleColor.Cyan, "[{0}]", Settings.Config.UpstreamProxy);
            }

            if (null != Settings.Config.RespondTo) {
                Console.Write("    Respond To");
                Print(ConsoleColor.Cyan, "[{0}]", Settings.Config.RespondTo);
            }

            if ((null != Settings.Config.RespondToName)
                && (0 != Settings.Config.RespondToName.Length))
            {
                Console.Write("    Respond To");
                Print(ConsoleColor.Cyan, "[{0}]", Settings.Config.RespondToName);
            }

            if (null != Settings.Config.DontRespondTo) {
                Console.Write("    Respond To");
                Print(ConsoleColor.Cyan, "[{0}]", Settings.Config.DontRespondTo);
            }

            if ((null != Settings.Config.DontRespondToName)
                && (0 < Settings.Config.DontRespondToName.Length))
            {
                Console.Write("    Respond To");
                Print(ConsoleColor.Cyan, "[{0}]", Settings.Config.DontRespondToName);
            }
            Console.WriteLine();
            return;
        }

        internal static IPAddress FindLocalIP(string Iface, IPAddress OURIP)
        {
            if ("ALL" == Iface) { return IPAddress.Any; }
            try {
                if (null != OURIP) { return OURIP; }
                using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)) {
                    // TODO : Implement the interface binding using [1] in our Global KB
                    // s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName. 25, Iface + '\0')
                    s.Connect("127.0.0.1", 9); // Discard service as per #RFC 863
                    return ((IPEndPoint)s.LocalEndPoint).Address;
                }
            }
            catch {
                Helpers.PrintError("[!] Error: {0}: Interface not found", Iface);
                Environment.Exit(255);
                return null; // Unreachable
            }
        }

        private static void PrintEnablement(bool enabled)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            if (enabled) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("    [ON]");
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("    [OFF]");
            }
            Console.ForegroundColor = oldColor;
            return;
        }

        internal static void PrintError(string message, params object[] args)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message, args);
            Console.ForegroundColor = oldColor;
            return;
        }

        internal static void PrintPrefix()
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            // Bold is missing.
            Console.Write("[+]");
            Console.ForegroundColor = oldColor;
            return;
        }

        private static void Print(ConsoleColor color, string format, params object[] args)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ForegroundColor = oldColor;
            return;
        }

        internal static void Write(this byte[] into, int offset, bool bigEndian, int value)
        {
            for(int index = 0; index < sizeof(int); index++) {
                into[index] = (bigEndian) ? (byte)((value & 0xFF000000) >> 24) : (byte)(value & 0xFF);
                if (bigEndian) { value <<= 8; }
                else { value >>= 8; }
            }
        }
    }
}
