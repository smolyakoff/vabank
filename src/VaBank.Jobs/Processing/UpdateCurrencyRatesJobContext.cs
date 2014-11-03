using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Processing;

namespace VaBank.Jobs.Processing
{
    public class UpdateCurrencyRatesJobContext : DefaultJobContext
    {
        public ICurrencyRateService CurrencyRateService { get; set; }
    }
}
