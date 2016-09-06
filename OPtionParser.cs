using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetResponder
{
    internal class OptionParser
    {
        internal void AddOption(string shortForm, string longForm, string action, string help,
            string propertyName, object defaultValue, string metaVar = null)
        {
            _commandLineOptions.Add(new CommandLineOption(shortForm, longForm, action, help,
                propertyName, defaultValue, metaVar));
        }

        internal void ParseCommandLineArguments(OptionSet into, string[] args)
        {
            int argsCount = args.Length;
            bool additionalDataExpected = false;
            for (int index = 0; index < argsCount; index++) {
                CommandLineOption matchingOption = null;
                string candidateKeyword = args[index].ToUpper();
                foreach(CommandLineOption scannedOption in _commandLineOptions) {
                    if ((scannedOption.ShortForm == candidateKeyword)
                        || (scannedOption.LongForm == candidateKeyword))
                    {
                        matchingOption = scannedOption;
                        break;
                    }
                }
                if (null == matchingOption) {
                    throw new ApplicationException(
                        string.Format("Unrecognized option {0}", candidateKeyword));
                }
            }
            if (additionalDataExpected) {
                throw new ApplicationException("Additional data expected.");
            }
            return;
        }

        private List<CommandLineOption> _commandLineOptions;

        private class CommandLineOption
        {
            internal CommandLineOption(string shortForm, string longForm, string action,
                string help, string propertyName, object defaultValue, string metaVar)
            {
                Action = action;
                ShortForm = shortForm.ToUpper();
                LongForm = longForm.ToUpper();
                Help = help;
                PropertyName = propertyName;
                DefaultValue = defaultValue;
                MetaVar = metaVar;
                return;
            }

            internal string Action { get; private set; }
            internal object DefaultValue { get; private set; }
            internal string Help { get; private set; }
            internal string LongForm { get; private set; }
            internal string MetaVar { get; private set; }
            internal string PropertyName { get; private set; }
            internal string ShortForm { get; private set; }
        }
    }
}
