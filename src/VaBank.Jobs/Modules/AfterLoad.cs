using Autofac;
using VaBank.Common.Events;
using VaBank.Jobs.Common;

namespace VaBank.Jobs.Modules
{
    internal class AfterLoad
    {
        public AfterLoad(IServiceBus serviceBus, ILifetimeScope scope)
        {
            serviceBus.Subscribe(new HangfireEventListener(scope));
        }
    }
}