using VaBank.Common.Data.Repositories;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Repositories;
using VaBank.Services.Common;

namespace VaBank.Services.Processing
{
    public class ExchangeRateServiceDependencies : IDependencyCollection
    {
        public IExchangeRateRepository ExchangeRates { get; set; }
        public IRepository<Currency> Currencies { get; set; }
        public ExchangeRateFactory ExchangeRateFactory { get; set; }
    }
}
