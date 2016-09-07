using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetResponder
{
    internal class OptionParser
    {
        internal void AddOption(string shortForm, string longForm, string action, string help,
            string propertyName, object defaultValue, string metaVar = null)
        {
            if (null == _commandLineOptions) {
                _commandLineOptions = new List<CommandLineOption>();
            }
            _commandLineOptions.Add(new CommandLineOption(shortForm, longForm, action, help,
                propertyName, defaultValue, metaVar));
        }

        internal void ParseCommandLineArguments(OptionSet into, string[] args)
        {
            int argsCount = args.Length;
            bool additionalDataExpected = false;
            Type intoType = into.GetType();
            for (int index = 0; index < argsCount; index++) {
                CommandLineOption matchingOption = null;
                string candidateKeyword = args[index].ToUpper();
                foreach (CommandLineOption scannedOption in _commandLineOptions) {
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
                PropertyInfo target = intoType.GetProperty(matchingOption.PropertyName,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (null == target) {
                    throw new ApplicationException(string.Format(
                        "Invalid description for option '{0}'. Can't find property '{1}'.",
                        matchingOption.ShortForm, matchingOption.PropertyName));
                }
                object value;
                switch (matchingOption.Action) {
                    case "store_true":
                        value = true;
                        break;
                    case "store":
                        if (++index >= argsCount) {
                            throw new ApplicationException(
                                string.Format("Value is missing for argument '{0}'.", candidateKeyword));
                        }
                        value = args[++index];
                        break;
                    default:
                        throw new ApplicationException(
                            string.Format("Unrecognized action '{0}' for argument '{1}'.",
                                matchingOption.Action, candidateKeyword));
                }
                try { target.SetValue(into, value); }
                catch (Exception e) {
                    string.Format("Error '{0}' while setting value '{1}' on argument '{2}'.",
                        e.Message, value, candidateKeyword);
                }
                if (additionalDataExpected) {
                    throw new ApplicationException("Additional data expected.");
                }
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
                ShortForm = (shortForm ?? string.Empty).ToUpper();
                LongForm = (longForm ?? string.Empty).ToUpper();
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
