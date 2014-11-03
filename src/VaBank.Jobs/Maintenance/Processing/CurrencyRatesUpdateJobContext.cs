using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Processing;

namespace VaBank.Jobs.Maintenance.Processing
{
    public class CurrencyRatesUpdateJobContext : DefaultJobContext
    {
        public ICurrencyRateService CurrencyRateService { get; set; }
    }
}
