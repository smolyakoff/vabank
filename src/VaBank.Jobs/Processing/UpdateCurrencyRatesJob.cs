using Autofac;
using VaBank.Jobs.Common;

namespace VaBank.Jobs.Processing
{
    [JobName("UpdateCurrencyRates")]
    public class UpdateCurrencyRatesJob : BaseJob<UpdateCurrencyRatesJobContext>
    {
        public UpdateCurrencyRatesJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(UpdateCurrencyRatesJobContext context)
        {
            context.CurrencyRateService.UpdateRates();
        }
    }
}
