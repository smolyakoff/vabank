using System;
using Autofac;
using Common.Logging;
using Common.Logging.Configuration;
using Hangfire;
using JVW.Logging.CommonLoggingNLogAdapter;
using Newtonsoft.Json;
using VaBank.Jobs.Common;
using VaBank.Jobs.Maintenance;

namespace VaBank.Jobs
{
    public class JobRunner
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
            RecurringJob.AddOrUpdate<KeepAliveJob>("KeepAlive", x => x.Execute(JobCancellationToken.Null), "*/10 * * * *");
        }
    }
}
