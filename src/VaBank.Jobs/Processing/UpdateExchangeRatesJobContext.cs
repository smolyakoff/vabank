using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Processing;

namespace VaBank.Jobs.Processing
{
    public class UpdateExchangeRatesJobContext : DefaultJobContext
    {
        public ICurrencyRateService CurrencyRateService { get; set; }
    }
}
