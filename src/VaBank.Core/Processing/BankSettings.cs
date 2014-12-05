using VaBank.Common.Util;
using VaBank.Common.Validation;

namespace VaBank.Core.Processing
{
    [Settings("VaBank.Core.Processing.BankSettings")]
    public class BankSettings
    {
        public BankSettings()
        {
            Location = "VABANK INTERNET / MINSK / BY";
            VaBankCode = "153001966";
        }

        public string Location { get; private set; }

        public string VaBankCode { get; private set; }

        public bool IsLocalLocation(string location)
        {
            Argument.NotNull(location, "location");
            return location.Trim().EndsWith("BY");
        }        
    }
}
