using VaBank.Common.Events;
using VaBank.Jobs.Common;

namespace VaBank.Jobs.Modules
{
    internal class AfterLoad
    {
        public AfterLoad(IServiceBus serviceBus)
        {
            serviceBus.Subscribe(new HangfireEventListener());
        }
    }
}