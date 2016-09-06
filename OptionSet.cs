using System.Net;

namespace NetResponder
{
    internal class OptionSet
    {
        internal bool Analyze { get; set; }
        internal bool BasicAuthEnabled { get; set; }
        internal bool FingerEnabled { get; set; }
        internal bool ForceWPADAuth { get; set; }
        internal string Interface { get; set; }
        internal bool LMEnabled { get; set; }
        internal string NBTNSDomain { get; set; }
        internal IPAddress OURIP { get; set; }
        internal bool UpstreamProxy { get; set; }
        internal bool Verbose { get; set; }
        internal bool WPADEnabled { get; set; }
        internal bool WRedirect { get; set; }
    }
}
