using Autofac;
using VaBank.Common.Events;

namespace VaBank.Jobs.Common
{
    public class EventDispatcherJob : ParameterJob<DefaultJobContext<IEvent>, IEvent>
    {
        public EventDispatcherJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(DefaultJobContext<IEvent> context)
        {
        }
    }
}
