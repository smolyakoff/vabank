using Autofac;
using VaBank.Jobs.Common;

namespace VaBank.Jobs.Processing
{
    [JobName("UpdateExchangeRates")]
    public class UpdateExchangeRatesJob : BaseJob<UpdateExchangeRatesJobContext>
    {
        public UpdateExchangeRatesJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(UpdateExchangeRatesJobContext context)
        {
            context.CurrencyRateService.UpdateRates();
            Logger.Info("Exchange rates were updated.");
        }
    }
}
