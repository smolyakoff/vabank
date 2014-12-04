using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Processing;

namespace VaBank.Jobs.Processing
{
    public class UpdateExchangeRatesJobContext : DefaultJobContext
    {
        public IExchangeRateService CurrencyRateService { get; set; }
    }
}
