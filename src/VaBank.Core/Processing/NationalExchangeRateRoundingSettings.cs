using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Util;
using VaBank.Common.Util.Math;
using VaBank.Common.Validation;

namespace VaBank.Core.Processing
{
    [Settings("VaBank.Core.Processing.NationalExchangeRateRoundingSettings")]
    public class NationalExchangeRateRoundingSettings
    {
        public NationalExchangeRateRoundingSettings()
        {
            NationalExchangeRateRounding = new List<NationalExchangeRateRounding>();
        }

        public List<NationalExchangeRateRounding> NationalExchangeRateRounding { get; set; }

        public Rounding GetRounding(string currencyISOName)
        {
            Argument.NotEmpty(currencyISOName, "currencyISOName");
            return NationalExchangeRateRounding.First(x => x.CurrencyISOName == currencyISOName).Rounding;
        }
    }

    public class NationalExchangeRateRounding
    {   
        public string CurrencyISOName { get; set; }

        public Rounding Rounding { get; set; }
    }
}
