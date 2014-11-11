using VaBank.Common.Validation;

namespace VaBank.Core.Processing
{
    public class ConversionRate
    {
        public ConversionRate(CurrencyConversion conversion, decimal rate)
        {
            Argument.Satisfies(rate, x => x > 0, "rate", "Rate should be greater than zero.");
            Conversion = conversion;
            Rate = rate;
        }

        public CurrencyConversion Conversion { get; private set; }

        public decimal Rate { get; private set; }
    }
}
