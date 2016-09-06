using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetResponder
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Helpers.DisplayBanner();
            string assemblyName = Assembly.GetEntryAssembly().GetName().Name;
            OptionParser parser = new OptionParser();
            // parser = optparse.OptionParser(usage = 'python %prog -I eth0 -w -r -f\nor:\npython %prog -I eth0 -wrf', version = settings.__version__, prog = assemblyName);
            parser.AddOption("-A", "--analyze", "store_true",
                "Analyze mode. This option allows you to see NBT-NS, BROWSER, LLMNR requests without responding.",
                "Analyze", false);
            parser.AddOption("-I", "--interface", "store",
                "Network interface to use",
                "Interface", null, "eth0");
            parser.AddOption("-i", "--ip", "store",
                "Local IP to use \033[1m\033[31m(only for OSX)\033[0m",
                "OURIP", null, "10.0.0.21");
            parser.AddOption("-b", "--basic", "store_true",
                "Return a Basic HTTP authentication. Default: NTLM",
                "Basic", false);
            parser.AddOption("-r", "--wredir", "store_true",
                "Enable answers for netbios wredir suffix queries. Answering to wredir will likely break stuff on the network. Default: False",
                "Wredirect", false);
            parser.AddOption("-d", "--NBTNSdomain", "store_true",
                "Enable answers for netbios domain suffix queries. Answering to domain suffixes will likely break stuff on the network. Default: False",
                "NBTNSDomain", false);
            parser.AddOption("-f", "--fingerprint", "store_true",
                "This option allows you to fingerprint a host that issued an NBT-NS or LLMNR query.",
                "Finger", false);
            parser.AddOption("-w", "--wpad", "store_true",
                "Start the WPAD rogue proxy server. Default value is False",
                "WPAD_On_Off", false);
            parser.AddOption("-u", "--upstream-proxy", "store",
                "Upstream HTTP proxy used by the rogue WPAD Proxy for outgoing requests (format: host:port)",
                "Upstream_Proxy", null);
            parser.AddOption("-F", "--ForceWpadAuth", "store_true",
                "Force NTLM / Basic authentication on wpad.dat file retrieval.This may cause a login prompt.Default: False",
                "Force_WPAD_Auth", false);
            parser.AddOption("--lm", string.Empty, "store_true",
                "Force LM hashing downgrade for Windows XP/2003 and earlier. Default: False",
                "LM_On_Off", false);
            parser.AddOption("-v", "--verbose", "store_true",
                "Increase verbosity.", "Verbose", null);
            // TODO
            OptionSet options = new OptionSet();
            parser.ParseCommandLineArguments(options, args);

            // TODO 
            if (false /* not os.geteuid() == 0*/) {
                Console.WriteLine("[!] Responder must be run as root.");
                Environment.Exit(255);
            }
            // Omit OsX only code.
            //if options.OURIP is None and IsOsX() is True:
            //    print "\n\033[1m\033[31mOSX detected, -i mandatory option is missing\033[0m\n"
            //    parser.print_help()
            //    exit(-1)

            Settings.Initialize();
            Settings.Config.Populate(options, args);
            Helpers.DisplayStartupMessage();
            Settings.Config.ExpandIPRanges();

            if (Settings.Config.AnalyzeMode) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                // TODO : bold
                Console.WriteLine("[i] Responder is in analyze mode. No NBT-NS, LLMNR, MDNS requests will be poisoned.");
                Console.ForegroundColor = ConsoleColor.Black;
            }

            try {
                List<Thread> threads = new List<Thread>();
        
                // Load (M)DNS, NBNS and LLMNR Poisoners
                //from poisoners.LLMNR import LLMNR
                //from poisoners.NBTNS import NBTNS
                //from poisoners.MDNS import MDNS

                //threads.append(Thread(target = serve_LLMNR_poisoner, args = ('', 5355, LLMNR,)))
		        //threads.append(Thread(target = serve_MDNS_poisoner, args = ('', 5353, MDNS,)))
		        //threads.append(Thread(target = serve_NBTNS_poisoner, args = ('', 137, NBTNS,)))

		        //# Load Browser Listener
		        //from servers.Browser import Browser

                //threads.append(Thread(target = serve_thread_udp_broadcast, args = ('', 138, Browser,)))

		        //if settings.Config.HTTP_On_Off:
		        //	from servers.HTTP import HTTP

                //threads.append(Thread(target = serve_thread_tcp, args = ('', 80, HTTP,)))

		        //if settings.Config.SSL_On_Off:
		        //	from servers.HTTP import HTTPS

                //threads.append(Thread(target = serve_thread_SSL, args = ('', 443, HTTPS,)))

		        //if settings.Config.WPAD_On_Off:
		        //	from servers.HTTP_Proxy import HTTP_Proxy

                //threads.append(Thread(target = serve_thread_tcp, args = ('', 3141, HTTP_Proxy,)))

		        //if settings.Config.SMB_On_Off:
		        //	if settings.Config.LM_On_Off:
		        //		from servers.SMB import SMB1LM

                //      threads.append(Thread(target = serve_thread_tcp, args = ('', 445, SMB1LM,)))
                //      threads.append(Thread(target = serve_thread_tcp, args = ('', 139, SMB1LM,)))
		        //	else:
		        //		from servers.SMB import SMB1

                //      threads.append(Thread(target = serve_thread_tcp, args = ('', 445, SMB1,)))
		        //		threads.append(Thread(target = serve_thread_tcp, args = ('', 139, SMB1,)))

		        //if settings.Config.Krb_On_Off:
		        //	from servers.Kerberos import KerbTCP, KerbUDP
        
                //  threads.append(Thread(target = serve_thread_udp, args = ('', 88, KerbUDP,)))
		        //	threads.append(Thread(target = serve_thread_tcp, args = ('', 88, KerbTCP,)))

		//if settings.Config.SQL_On_Off:
		//	from servers.MSSQL import MSSQL

  //          threads.append(Thread(target = serve_thread_tcp, args = ('', 1433, MSSQL,)))

		//if settings.Config.FTP_On_Off:
		//	from servers.FTP import FTP

  //          threads.append(Thread(target = serve_thread_tcp, args = ('', 21, FTP,)))

		//if settings.Config.POP_On_Off:
		//	from servers.POP3 import POP3

  //          threads.append(Thread(target = serve_thread_tcp, args = ('', 110, POP3,)))

		//if settings.Config.LDAP_On_Off:
		//	from servers.LDAP import LDAP

  //          threads.append(Thread(target = serve_thread_tcp, args = ('', 389, LDAP,)))

		//if settings.Config.SMTP_On_Off:
		//	from servers.SMTP import ESMTP

        //  threads.append(Thread(target = serve_thread_tcp, args = ('', 25, ESMTP,)))
		//	threads.append(Thread(target = serve_thread_tcp, args = ('', 587, ESMTP,)))

		//if settings.Config.IMAP_On_Off:
		//	from servers.IMAP import IMAP

        //  threads.append(Thread(target = serve_thread_tcp, args = ('', 143, IMAP,)))

		//if settings.Config.DNS_On_Off:
		//	from servers.DNS import DNS, DNSTCP
        
        //  threads.append(Thread(target = serve_thread_udp, args = ('', 53, DNS,)))
		//	threads.append(Thread(target = serve_thread_tcp, args = ('', 53, DNSTCP,)))

                foreach(Thread thread in threads) {
                    thread.IsBackground = true;
                    thread.Start();
                }
                Helpers.PrintPrefix();
                Console.WriteLine(" Listening for events...");
                while (true) { Thread.Sleep(1000); }
            }
            catch {
                // except KeyboardInterrupt:
                Console.WriteLine();
                Helpers.PrintPrefix();
                Console.WriteLine(" Exiting...");
                Environment.Exit(0);
            }
        }
    }
}
