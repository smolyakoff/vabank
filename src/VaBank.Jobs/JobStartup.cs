using Common.Logging;
using Common.Logging.Configuration;
using JVW.Logging.CommonLoggingNLogAdapter;
using VaBank.Jobs.Common;
using VaBank.Jobs.Maintenance;

namespace VaBank.Jobs
{
    public class JobStartup
    {
        public void Start()
        {
            ConfigureLogging();
            RegisterRecurring();
        }

        private static void ConfigureLogging()
        {
            var properties = new NameValueCollection();
            properties["configType"] = "INLINE";
            LogManager.Adapter = new NLogFactoryAdapter(properties);
        }

        private static void RegisterRecurring()
        {
            #if !DEBUG
                VabankJob.AddOrUpdateRecurring<KeepAliveJob>("KeepAlive", "*/10 * * * *");
            #endif
        }
    }
}
