using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NetResponder
{
    internal class ConfigurationParser
    {
        internal void Read(StreamReader reader)
        {
        }

        internal string GetValue(string section, string item)
        {
            throw new NotImplementedException();
        }

        internal bool GetBooleanValue(string section, string item)
        {
            string rawValue = GetValue(section, item);
            return bool.Parse(rawValue);
        }
    }
}
