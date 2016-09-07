using System;
using System.Collections.Generic;
using System.IO;

namespace NetResponder
{
    internal class ConfigurationParser
    {
        internal void AcquireContent(StreamReader reader)
        {
            List<string> allLines = new List<string>();
            while (true) {
                string currentLine = reader.ReadLine();
                if (null == currentLine) { break; }
                if (!string.IsNullOrEmpty(currentLine)) {
                    allLines.Add(currentLine);
                }
            }
            _perSectionValues = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> currentSection = null;
            int lineNumber = 0;
            foreach (string line in allLines) {
                ++lineNumber;
                string trimmedLine = line.Trim();
                // Remove comments
                if (trimmedLine.StartsWith(";")) { continue; }
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]")) {
                    if (2 == trimmedLine.Length) {
                        throw new ApplicationException(string.Format(
                            "Invalid section name on line {0}.", lineNumber));
                    }
                    currentSection = new Dictionary<string, string>();
                    _perSectionValues.Add(
                        trimmedLine.Substring(1, trimmedLine.Length - 2),
                        currentSection);
                    continue;
                }
                if (null == currentSection) {
                    throw new ApplicationException(string.Format(
                        "Expecting a section header at line #{0}. Found {1}.",
                        lineNumber, trimmedLine));
                }
                int splitIndex = trimmedLine.IndexOf('=');
                if (0 >= splitIndex) {
                    throw new ApplicationException(string.Format(
                        "Ill-formed config line at line #{0}. Found {1}.",
                        lineNumber, trimmedLine));
                }
                string key = trimmedLine.Substring(0, splitIndex);
                string value = (trimmedLine.Length > (splitIndex + 1))
                    ? trimmedLine.Substring(splitIndex + 1).TrimStart()
                    : string.Empty;
                try {
                    currentSection.Add(key.Trim(), value);
                }
                catch {
                    throw new ApplicationException(string.Format(
                        "Found duplicate key '{0}' at line {1}.",
                        trimmedLine, lineNumber));
                }
            }
        }

        internal bool GetBooleanValue(string section, string item)
        {
            string rawValue = GetValue(section, item).Trim();
            switch (rawValue.ToUpper()) {
                case "ON":
                    return true;
                case "OFF":
                    return false;
                default:
                    return bool.Parse(rawValue);
            }
        }

        internal string GetValue(string section, string key)
        {
            Dictionary<string, string> items;
            if (!_perSectionValues.TryGetValue(section.Trim(), out items)) {
                throw new ApplicationException(string.Format(
                    "Unknown section '{0}'", section));
            }
            string result;
            return (items.TryGetValue(key.Trim(), out result)) ? result : null;
        }

        private Dictionary<string, Dictionary<string, string>> _perSectionValues;
    }
}
