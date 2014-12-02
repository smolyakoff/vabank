using System.Collections.Generic;
using VaBank.Common.Util;

namespace VaBank.Core.Processing
{
    [Settings("VaBank.Core.Processing.NationalExchangeRateRoundingSettings")]
    public class NationalExchangeRateRoundingSettings
    {
        public List<NationalExchangeRateRounding> NationalExchangeRateRounding { get; private set; }
    }

    public class NationalExchangeRateRounding
    {
        private NationalExchangeRateRounding()
        {
        }      

        public string CurrencyISOName { get; private set; }

        public MoneyRounding Rounding { get; private set; }
    }
}
