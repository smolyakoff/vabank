using System;
using Autofac;
using VaBank.Common.Events;
using VaBank.Jobs.Common;

namespace VaBank.Jobs.Modules
{
    internal class RegisterHangfireListener : IStartable
    {
        private readonly IServiceBus _serviceBus;

        private readonly ILifetimeScope _rootScope;

        public RegisterHangfireListener(IServiceBus serviceBus, ILifetimeScope scope)
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
            _serviceBus.Subscribe(new HangfireEventListener(_rootScope));
        }
    }
}