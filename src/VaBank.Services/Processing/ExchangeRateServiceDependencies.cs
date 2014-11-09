using VaBank.Core.Processing.Repositories;
using VaBank.Services.Common;

namespace VaBank.Services.Processing
{
    public class ExchangeRateServiceDependencies : IDependencyCollection
    {
        public IExchangeRateRepository ExchangeRates { get; set; }
    }
}
