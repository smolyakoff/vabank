using VaBank.Common.Util;

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
    }
}
