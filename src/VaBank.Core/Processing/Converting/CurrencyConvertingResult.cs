using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Processing.Converting
{
    public class CurrencyConvertingResult
    {
        public Currency Currency { get; internal set; }

        public decimal Amount { get; internal set; }
    }
}
