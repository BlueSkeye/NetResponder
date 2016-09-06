﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetResponder
{
    internal class Settings
    {
        internal Settings()
        {
            this.ResponderPATH =
                new FileInfo(Assembly.GetEntryAssembly().Location).Directory.FullName;
            BindTo = IPAddress.None;
            //		self.Bind_To = "0.0.0.0"
        }

        internal FileInfo AnalyzeLogFile { get; private set; }
        internal bool AnalyzeMode { get; private set; }
        internal bool AutoIgnore { get; private set; }
        internal bool BasicAuthEnabled { get; private set; }
        internal IPAddress BindTo { get; private set; }
        internal bool CaptureMultipleCredentials { get; private set; }
        internal byte[] Challenge { get; private set; }
        internal string[] CommandLine { get; private set; }
        internal FileInfo DatabaseFile { get; private set; }
        internal bool DNSEnabled { get; private set; }
        internal FileInfo ExeDlName { get; private set; }
        internal FileInfo ExeFilename { get; private set; }
        internal bool FingerEnabled { get; private set; }
        internal bool ForceWPADAuth { get; private set; }
        internal bool FTPEnabled { get; private set; }
        internal FileInfo FTPLog { get; private set; }
        internal FileInfo HtmlFilename { get; private set; }
        internal string HtmlToInject { get; private set; }
        internal FileInfo HTTPBasicLog { get; private set; }
        internal bool HTTPEnabled { get; private set; }
        internal FileInfo HTTPNTLMv1Log { get; private set; }
        internal FileInfo HTTPNTLMv2Log { get; private set; }
        internal bool IMAPEnabled { get; private set; }
        internal FileInfo IMAPLog { get; private set; }
        internal bool KrbEnabled { get; private set; }
        internal FileInfo KerberosLog { get; private set; }
        internal FileInfo LDAPClearLog { get; private set; }
        internal bool LDAPEnabled { get; private set; }
        internal FileInfo LDAPNTLMv1Log { get; private set; }
        internal bool LMEnabled { get; private set; }
        internal DirectoryInfo LogDir { get; private set; }
        internal FileInfo MSSQLClearLog { get; private set; }
        internal FileInfo MSSQLNTLMv1Log { get; private set; }
        internal FileInfo MSSQLNTLMv2Log { get; private set; }
        internal string NBTNSDomain { get; private set; }
        internal string NumCha1 { get; private set; }
        internal FileInfo PoisonersLogFile { get; private set; }
        internal FileInfo POP3Log { get; private set; }
        internal bool POPEnabled { get; private set; }
        internal string ResponderPATH { get; private set; }
        internal bool ServeAlways { get; private set; }
        internal bool ServeExe { get; private set; }
        internal bool ServeHtml { get; private set; }
        internal FileInfo SessionLogFile { get; private set; }
        internal FileInfo SMBClearLog { get; private set; }
        internal bool SMBEnabled { get; private set; }
        internal FileInfo SMBNTLMv1Log { get; private set; }
        internal FileInfo SMBNTLMv2Log { get; private set; }
        internal FileInfo SMBNTLMSSPv1Log { get; private set; }
        internal FileInfo SMBNTLMSSPv2Log { get; private set; }
        internal FileInfo SMTPClearLog { get; private set; }
        internal bool SMTPEnabled { get; private set; }
        internal bool SQLEnabled { get; private set; }
        internal bool SSLEnabled { get; private set; }
        internal bool UpstreamProxy { get; private set; }
        internal bool Verbose { get; private set; }
        internal bool WPADAuthEnabled { get; private set; }
        internal bool WPADEnabled { get; private set; }
        internal string WPADScript { get; private set; }

        internal static void Initialize()
        {
            Config = new Settings();
        }

        public override string ToString()
        {
            string ret = "Settings class:\n";
            PropertyInfo[] allProperties =
                this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (PropertyInfo attr in allProperties) {
                object rawValue = attr.GetMethod.Invoke(this, null);
                string value = (null == rawValue) ? "<NULL>" : rawValue.ToString();
                ret += string.Format("    Settings.{0} = {1}\n", attr, value);
            }
            return ret;
        }

        internal void ExpandIPRanges()
        {
            throw new NotImplementedException();
            //        def expand_ranges(lst):
            //            ret = []
            //			for l in lst:
            //				tab = l.split(".")
            //				x = {}
            //				i = 0
            //				for byte in tab:
            //					if "-" not in byte:
            //						x[i] = x[i + 1] = int(byte)
            //					else:
            //						b = byte.split("-")

            //                        x[i] = int(b[0])
            //						x[i + 1] = int(b[1])
            //					i += 2
            //				for a in range(x[0], x[1]+1):
            //					for b in range(x[2], x[3]+1):
            //						for c in range(x[4], x[5]+1):
            //							for d in range(x[6], x[7]+1):

            //                                ret.append("%d.%d.%d.%d" % (a, b, c, d);
            //			return ret
            //        self.RespondTo = expand_ranges(self.RespondTo)
            //        self.DontRespondTo = expand_ranges(self.DontRespondTo)
        }

        internal void Populate(object options)
        {
            // if options.Interface is None and utils.IsOsX() is False:
            //      print utils.color("Error: -I <if> mandatory option is missing", 1)
            //      sys.exit(-1)

            // Config parsing
            ConfigurationParser parser = new ConfigurationParser();
            string configurationFilePath = Path.Combine(this.ResponderPATH, "Responder.conf");
            using (FileStream input = File.Open(configurationFilePath, FileMode.Open, FileAccess.Read)) {
                using (StreamReader reader = new StreamReader(input)) {
                    parser.Read(reader);
                }
            }

            // # Servers
            this.HTTPEnabled = parser.GetBooleanValue("Responder Core", "HTTP");
            this.SSLEnabled = parser.GetBooleanValue("Responder Core", "HTTPS");
            this.SMBEnabled = parser.GetBooleanValue("Responder Core", "SMB");
            this.SQLEnabled = parser.GetBooleanValue("Responder Core", "SQL");
            this.FTPEnabled = parser.GetBooleanValue("Responder Core", "FTP");
            this.POPEnabled = parser.GetBooleanValue("Responder Core", "POP");
            this.IMAPEnabled = parser.GetBooleanValue("Responder Core", "IMAP");
            this.SMTPEnabled = parser.GetBooleanValue("Responder Core", "SMTP");
            this.LDAPEnabled = parser.GetBooleanValue("Responder Core", "LDAP");
            this.DNSEnabled = parser.GetBooleanValue("Responder Core", "DNS");
            this.KrbEnabled = parser.GetBooleanValue("Responder Core", "Kerberos");

            // # Db File
            this.DatabaseFile = new FileInfo(
                Path.Combine(this.ResponderPATH, parser.GetValue("Responder Core", "Database")));

            // # Log Files
            this.LogDir = new DirectoryInfo(Path.Combine(this.ResponderPATH, "logs"));
            if (!this.LogDir.Exists) {
                this.LogDir.Create();
            }

            this.SessionLogFile = new FileInfo(
                Path.Combine(this.LogDir.FullName, parser.GetValue("Responder Core", "SessionLog")));
            this.PoisonersLogFile = new FileInfo(
                Path.Combine(this.LogDir.FullName, parser.GetValue("Responder Core", "PoisonersLog")));
            this.AnalyzeLogFile = new FileInfo(
                Path.Combine(this.LogDir.FullName, parser.GetValue("Responder Core", "AnalyzeLog")));
            this.FTPLog = new FileInfo(Path.Combine(this.LogDir.FullName, "FTP-Clear-Text-Password-%s.txt"));
            this.IMAPLog = new FileInfo(Path.Combine(this.LogDir.FullName, "IMAP-Clear-Text-Password-%s.txt"));
            this.POP3Log = new FileInfo(Path.Combine(this.LogDir.FullName, "POP3-Clear-Text-Password-%s.txt"));
            this.HTTPBasicLog = new FileInfo(Path.Combine(this.LogDir.FullName, "HTTP-Clear-Text-Password-%s.txt"));
            this.LDAPClearLog = new FileInfo(Path.Combine(this.LogDir.FullName, "LDAP-Clear-Text-Password-%s.txt"));
            this.SMBClearLog = new FileInfo(Path.Combine(this.LogDir.FullName, "SMB-Clear-Text-Password-%s.txt"));
            this.SMTPClearLog = new FileInfo(Path.Combine(this.LogDir.FullName, "SMTP-Clear-Text-Password-%s.txt"));
            this.MSSQLClearLog = new FileInfo(Path.Combine(this.LogDir.FullName, "MSSQL-Clear-Text-Password-%s.txt"));

            this.LDAPNTLMv1Log = new FileInfo(Path.Combine(this.LogDir.FullName, "LDAP-NTLMv1-Client-%s.txt"));
            this.HTTPNTLMv1Log = new FileInfo(Path.Combine(this.LogDir.FullName, "HTTP-NTLMv1-Client-%s.txt"));
            this.HTTPNTLMv2Log = new FileInfo(Path.Combine(this.LogDir.FullName, "HTTP-NTLMv2-Client-%s.txt"));
            this.KerberosLog = new FileInfo(Path.Combine(this.LogDir.FullName, "MSKerberos-Client-%s.txt"));
            this.MSSQLNTLMv1Log = new FileInfo(Path.Combine(this.LogDir.FullName, "MSSQL-NTLMv1-Client-%s.txt"));
            this.MSSQLNTLMv2Log = new FileInfo(Path.Combine(this.LogDir.FullName, "MSSQL-NTLMv2-Client-%s.txt"));
            this.SMBNTLMv1Log = new FileInfo(Path.Combine(this.LogDir.FullName, "SMB-NTLMv1-Client-%s.txt"));
            this.SMBNTLMv2Log = new FileInfo(Path.Combine(this.LogDir.FullName, "SMB-NTLMv2-Client-%s.txt"));
            this.SMBNTLMSSPv1Log = new FileInfo(Path.Combine(this.LogDir.FullName, "SMB-NTLMSSPv1-Client-%s.txt"));
            this.SMBNTLMSSPv2Log = new FileInfo(Path.Combine(this.LogDir.FullName, "SMB-NTLMSSPv2-Client-%s.txt"));

            // # HTTP Options
            this.ServeExe = parser.GetBooleanValue("HTTP Server", "Serve-Exe");
            this.ServeAlways = parser.GetBooleanValue("HTTP Server", "Serve-Always");
            this.ServeHtml = parser.GetBooleanValue("HTTP Server", "Serve-Html");
            this.HtmlFilename = new FileInfo(parser.GetValue("HTTP Server", "HtmlFilename"));
            this.ExeFilename = new FileInfo(parser.GetValue("HTTP Server", "ExeFilename"));
            this.ExeDlName = new FileInfo(parser.GetValue("HTTP Server", "ExeDownloadName"));
            this.WPADScript = parser.GetValue("HTTP Server", "WPADScript");
            this.HtmlToInject = parser.GetValue("HTTP Server", "HtmlToInject");

            if (!this.HtmlFilename.Exists) {
                Helpers.PrintError("/!\\ Warning: {0}: file not found", this.HtmlFilename.FullName);
            }
            if (!this.ExeFilename.Exists) {
                Helpers.PrintError("/!\\ Warning: {0}: file not found", this.ExeFilename.FullName);
            }

            // # SSL Options
            // self.SSLKey  = config.get("HTTPS Server", "SSLKey")
            // self.SSLCert = config.get("HTTPS Server", "SSLCert")

            // # Respond to hosts
            // self.RespondTo         = filter(None, [x.upper().strip() for x in config.get("Responder Core", "RespondTo").strip().split(",")])
            // self.RespondToName     = filter(None, [x.upper().strip() for x in config.get("Responder Core", "RespondToName").strip().split(",")])
            // self.DontRespondTo     = filter(None, [x.upper().strip() for x in config.get("Responder Core", "DontRespondTo").strip().split(",")])
            // self.DontRespondToName = filter(None, [x.upper().strip() for x in config.get("Responder Core", "DontRespondToName").strip().split(",")])

            // # Auto Ignore List
            // self.AutoIgnore                 = self.toBool(config.get("Responder Core", "AutoIgnoreAfterSuccess");
            // self.CaptureMultipleCredentials = self.toBool(config.get("Responder Core", "CaptureMultipleCredentials");
            // self.AutoIgnoreList             = []

            //# CLI options
            // self.LM_On_Off       = options.LM_On_Off
            // self.WPAD_On_Off     = options.WPAD_On_Off
            // self.Wredirect       = options.Wredirect
            // self.NBTNSDomain     = options.NBTNSDomain
            // self.Basic           = options.Basic
            // self.Finger_On_Off   = options.Finger
            // self.Interface       = options.Interface
            // self.OURIP           = options.OURIP
            // self.Force_WPAD_Auth = options.Force_WPAD_Auth
            // self.Upstream_Proxy  = options.Upstream_Proxy
            // self.AnalyzeMode     = options.Analyze
            // self.Verbose         = options.Verbose
            // self.CommandLine     = str(sys.argv)

            if (null == HtmlToInject) {
                HtmlToInject = string.Empty;
            }

            // self.Bind_To = utils.FindLocalIP(self.Interface, self.OURIP)
            // self.IP_aton         = socket.inet_aton(self.Bind_To)
            // self.Os_version      = sys.platform

            // # Set up Challenge
            this.NumCha1 = parser.GetValue("Responder Core", "Challenge");

            if (16 != this.NumCha1.Length) {
                Helpers.PrintError(
                    "[!] The challenge must be exactly 16 chars long.\nExample: 1122334455667788");
                Environment.Exit(255);
            }

            this.Challenge = new byte[8];
            for(int index = 0; index < 16; index += 2) {
                this.Challenge[index / 2] =
                    byte.Parse(this.NumCha1.Substring(index, 2), NumberStyles.AllowHexSpecifier);
            }

            // # Set up logging
            // logging.basicConfig(filename=self.SessionLogFile, level=logging.INFO, format="%(asctime)s - %(message)s", datefmt="%m/%d/%Y %I:%M:%S %p")
            Logging.Warning("Responder Started: {0}", this.CommandLine);
            Logging.Warning("Responder Config: {0}", this);

            //	Formatter = logging.Formatter("%(asctime)s - %(message)s")
            //	PLog_Handler = Logging.FileHandler(self.PoisonersLogFile, "w")
            //	ALog_Handler = Logging.FileHandler(self.AnalyzeLogFile, "a")
            //	PLog_Handler.setLevel(logging.INFO)
            //	ALog_Handler.setLevel(logging.INFO)
            //	PLog_Handler.setFormatter(Formatter)
            //	ALog_Handler.setFormatter(Formatter)

            //this.PoisonersLogger = Logging.getLogger("Poisoners Log");
            //this.PoisonersLogger.AddHandler(PLog_Handler);

            //	self.AnalyzeLogger = Logging.getLogger("Analyze Log")
            //	self.AnalyzeLogger.AddHandler(ALog_Handler)
            throw new NotImplementedException();
        }

        internal static Settings Config { get; private set; }
    }
}
