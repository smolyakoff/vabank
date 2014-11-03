using Hangfire;
using System.Configuration;
using VaBank.Jobs.Common;
using VaBank.Jobs.Maintenance;
using VaBank.Jobs.Maintenance.Processing;

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
            VabankJob.AddOrUpdateRecurring<KeepAliveJob, DefaultJobContext>("KeepAlive", "*/10 * * * *");

            var ratesCron = ConfigurationManager.AppSettings["updateCurrencyTimeUtc"] ?? Cron.Daily(21);
            VabankJob.AddOrUpdateRecurring<CurrencyRatesUpdateJob, CurrencyRatesUpdateJobContext>("UpdateCurrencyRates", ratesCron);
#endif
        }
    }
}
