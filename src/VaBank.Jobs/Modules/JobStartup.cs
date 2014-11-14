using Autofac;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Events;
using VaBank.Jobs.Common;
using VaBank.Jobs.Common.Settings;
using VaBank.Jobs.Maintenance;
using VaBank.Jobs.Processing;

namespace VaBank.Jobs.Modules
{
    public class JobStartup
    {
        private readonly IServiceBus _serviceBus;

        private readonly ILifetimeScope _rootScope;

        public JobStartup(ILifetimeScope rootScope)
        {
            if (rootScope == null)
            {
                throw new ArgumentNullException("rootScope");
            }
            _serviceBus = rootScope.Resolve<IServiceBus>();
            _rootScope = rootScope;
        }

        public void Start()
        {
            var automapperProfiles = _rootScope.Resolve<IEnumerable<Profile>>().ToList();
            automapperProfiles.ForEach(Mapper.AddProfile);
            _serviceBus.Subscribe(new HangfireEventListener(_rootScope));

            RegisterRecurring(); 
        }

        private void RegisterRecurring()
        {
            using (var scope = _rootScope.BeginLifetimeScope())
            {
                var provider = scope.Resolve<JobSettingsProvider>();
                RegisterRecurring<KeepAliveJob, DefaultJobContext>(provider, "KeepAlive");
                RegisterRecurring<UpdateExchangeRatesJob, UpdateExchangeRatesJobContext>(provider, "UpdateExchangeRates");
            }
        }

        private void RegisterRecurring<TJob, TJobContext>(JobSettingsProvider settingsProvider, string jobName) 
            where TJob : BaseJob<TJobContext> 
            where TJobContext : class, IJobContext
        {
            var key = string.Format("Recurring.{0}", jobName);
            var settings = settingsProvider.SurelyGetSettings<RecurringJobSettings>(key);
            if (settings.Enabled)
            {
                VabankJob.AddOrUpdateRecurring<TJob, TJobContext>(jobName, settings.Cron);
            }
        }
    }
}