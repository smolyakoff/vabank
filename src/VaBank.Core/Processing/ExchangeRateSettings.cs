using System.Collections.Generic;
using VaBank.Common.Util;
using VaBank.Common.Validation;

namespace VaBank.Core.Processing
{
    [Settings("VaBank.Core.Processing.ExchangeRateSettings")]
    public class ExchangeRateSettings
    {
        public ExchangeRateSettings()
        {
            NationalCurrency = "BYR";
            CurrencyPriority = new SortedList<string, int>
            {
                { NationalCurrency, 0 },
                { "EUR", 1}
            };
            BuyRateCoefficientRange = new Range<double>(0.999, 1);
            SellRateCoefficientRange = new Range<double>(1.006, 1.01);
            NationalExchangeRateRounding = new MoneyRounding(new IntegerRounding(IntegerRoundingMode.Ceiling, 10));
        }

        public string NationalCurrency { get; private set; }

        public MoneyRounding NationalExchangeRateRounding { get; private set; }

        public SortedList<string, int> CurrencyPriority { get; private set; }

        public Range<double> BuyRateCoefficientRange { get; private set; }

        public Range<double> SellRateCoefficientRange { get; private set; }

        public int GetPriority(string currencyISOName)
        {
            Argument.NotEmpty(currencyISOName, "currencyISOName");

            return CurrencyPriority.ContainsKey(currencyISOName)
                ? CurrencyPriority[currencyISOName]
                : int.MaxValue;
        }
    }
}
