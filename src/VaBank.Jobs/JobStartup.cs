using Hangfire;
using VaBank.Jobs.Common;
using VaBank.Jobs.Maintenance;
using VaBank.Jobs.Maintenance.Accounting;

namespace VaBank.Jobs
{
    public class JobStartup
    {
        public void Start()
        {
            RegisterRecurring();
        }

        private static void RegisterRecurring()
        {
            #if !DEBUG
            VabankJob.AddOrUpdateRecurring<KeepAliveJob>("KeepAlive", "*/10 * * * *");
            VabankJob.AddOrUpdateRecurring<CurrencyRatesUpdateJob>("UpdateCurrencyRates", Cron.Daily(21));
            #endif
        }
    }
}
