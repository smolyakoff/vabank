using Hangfire;
using System.Configuration;
using VaBank.Jobs.Common;
using VaBank.Jobs.Maintenance;
using VaBank.Jobs.Processing;

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
            //TODO: uncomment when job is actually implemented
            //var jobConfig = _jobConfigProvider.Get<ReccuringJobConfig>("UpdateCurrencyRates");
            //var cronExpression = jobConfig == null ? Cron.Daily(21) : jobConfig.CronExpression;
            //VabankJob.AddOrUpdateRecurring<UpdateCurrencyRatesJob, UpdateCurrencyRatesJobContext>("UpdateCurrencyRates", cronExpression);
            
#if !DEBUG
          VabankJob.AddOrUpdateRecurring<KeepAliveJob, DefaultJobContext>("KeepAlive", "*/10 * * * *");  
#endif
        }
    }
}
