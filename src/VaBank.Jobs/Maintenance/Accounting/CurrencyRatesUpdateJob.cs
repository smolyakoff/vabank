using Autofac;
using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Processing;

namespace VaBank.Jobs.Maintenance.Accounting
{
    public class CurrencyRatesUpdateJob : BaseJob<DefaultJobContext>
    {
        public CurrencyRatesUpdateJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(DefaultJobContext context)
        {
            var service = RootScope.Resolve<ICurrencyRateService>();
            service.UpdateRates();
        }
    }
}
