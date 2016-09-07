using System;
using System.IO;
using System.Text;

namespace NetResponder
{
    internal static class Logging
    {
        private static string FormatMessage(Level level, string message, params object[] args)
        {
            string body = string.Format(message, args);
            return string.Format("{0} - {1}",
                DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), body);
        }

        private static void Log(string fullMessage)
        {
            using (FileStream output = File.Open(LogFile.FullName, FileMode.OpenOrCreate, FileAccess.Write)) {
                using (StreamWriter writer = new StreamWriter(output, Encoding.UTF8)) {
                    writer.WriteLine(fullMessage);
                }
            }
        }

        internal static void Warning(string message, params object[] args)
        {
            if (Level.Warning > CurrentLevel) { return; }
            Log(FormatMessage(Level.Warning, message, args));
            return;
        }

        internal static FileInfo LogFile;
        internal static Level CurrentLevel = Level.Info;

        internal enum Level
        {
            Critical,
            Error,
            Warning,
            Info,
            Debug
        }
    }
}
