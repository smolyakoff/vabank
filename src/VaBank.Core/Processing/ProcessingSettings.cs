using VaBank.Common.Util;
using VaBank.Common.Validation;

namespace VaBank.Core.Processing
{
    [Settings("VaBank.Core.Processing.ProcessingSettings")]
    public class ProcessingSettings
    {
        public ProcessingSettings()
        {
            Location = "VABANK INTERNET / MINSK / BY";
        }

        public string Location { get; private set; }

        public bool IsLocalLocation(string location)
        {
            Argument.NotNull(location, "location");
            return location.Trim().EndsWith("BY");
        }
    }
}
