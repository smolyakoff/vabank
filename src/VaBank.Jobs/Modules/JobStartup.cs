using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutoMapper;
using VaBank.Common.Events;
using VaBank.Jobs.Common;

namespace VaBank.Jobs.Modules
{
    internal class JobStartup : IStartable
    {
        private readonly IServiceBus _serviceBus;

        private readonly ILifetimeScope _rootScope;

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
        }
    }
}