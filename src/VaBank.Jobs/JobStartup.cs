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
            //TODO : normal settings, not app settings from web.config
            //TODO: uncommment when job is actually implemented
            //var ratesCron = ConfigurationManager.AppSettings["updateCurrencyTimeUtc"] ?? Cron.Daily(21);
            //VabankJob.AddOrUpdateRecurring<UpdateCurrencyRatesJob, UpdateCurrencyRatesJobContext>("UpdateCurrencyRates", ratesCron);
            
#if !DEBUG
          VabankJob.AddOrUpdateRecurring<KeepAliveJob, DefaultJobContext>("KeepAlive", "*/10 * * * *");  
#endif
        }
    }
}
