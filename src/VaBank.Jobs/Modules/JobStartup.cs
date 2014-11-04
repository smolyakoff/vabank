using Autofac;
using AutoMapper;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Events;
using VaBank.Jobs.Common;
using VaBank.Jobs.Configuration;
using VaBank.Jobs.Processing;

namespace VaBank.Jobs.Modules
{
    internal class JobStartup : IStartable
    {
        private readonly IServiceBus _serviceBus;

        private readonly ILifetimeScope _rootScope;

        private readonly IJobConfigProvider _jobConfigProvider;

        public JobStartup(IServiceBus serviceBus, ILifetimeScope scope)
        {
            if (serviceBus == null)
            {
                throw new ArgumentNullException("serviceBus");
            }
            if (scope == null)
            {
                throw new ArgumentNullException("scope");
            }
            _serviceBus = serviceBus;
            _rootScope = scope;
        }

        public void Start()
        {
            var automapperProfiles = _rootScope.Resolve<IEnumerable<Profile>>().ToList();
            automapperProfiles.ForEach(Mapper.AddProfile);
            _serviceBus.Subscribe(new HangfireEventListener(_rootScope));

            //Recurring jobs

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