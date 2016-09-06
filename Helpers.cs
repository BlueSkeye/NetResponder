using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetResponder
{
    internal static class Helpers
    {
        internal static void DisplayBanner()
        {
            string banner = 
                "                                         __\\" +
                "  .----.-----.-----.-----.-----.-----.--|  |.-----.----.\n" +
                "  |   _|  -__|__ --|  _  |  _  |     |  _  ||  -__|   _|\n" +
                "  |__| |_____|_____|   __|_____|__|__|_____||_____|__|\n" +
                "                   |__|";
            Console.Write(banner);
            Console.WriteLine();
            Console.Write("           ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("NBT-NS, LLMNR & MDNS %s", Constants.Version);
            Console.ForegroundColor = ConsoleColor.Black;
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

            //   print '    %-27s' % "Responder NIC" + color('[%s]' % settings.Config.Interface, 5, 1)
            //   print '    %-27s' % "Responder IP" + color('[%s]' % settings.Config.Bind_To, 5, 1)
            //   print '    %-27s' % "Challenge set" + color('[%s]' % settings.Config.NumChal, 5, 1)

            //   if settings.Config.Upstream_Proxy:
            //	print '    %-27s' % "Upstream Proxy" + color('[%s]' % settings.Config.Upstream_Proxy, 5, 1)

            //   if len(settings.Config.RespondTo):
            //	print '    %-27s' % "Respond To" + color(str(settings.Config.RespondTo), 5, 1)

            //   if len(settings.Config.RespondToName):
            //	print '    %-27s' % "Respond To Names" + color(str(settings.Config.RespondToName), 5, 1)

            //   if len(settings.Config.DontRespondTo):
            //	print '    %-27s' % "Don't Respond To" + color(str(settings.Config.DontRespondTo), 5, 1)

            //   if len(settings.Config.DontRespondToName):
            //	print '    %-27s' % "Don't Respond To Names" + color(str(settings.Config.DontRespondToName), 5, 1)

            Console.WriteLine();
            throw new NotImplementedException();
            return;
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
    }
}
