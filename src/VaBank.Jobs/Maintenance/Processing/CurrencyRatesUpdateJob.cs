using Autofac;
using VaBank.Jobs.Common;

namespace VaBank.Jobs.Maintenance.Processing
{
    public class CurrencyRatesUpdateJob : BaseJob<CurrencyRatesUpdateJobContext>
    {
        public CurrencyRatesUpdateJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(CurrencyRatesUpdateJobContext context)
        {
            context.CurrencyRateService.UpdateRates();
        }
    }
}
