using Autofac;
using VaBank.Common.Events;

namespace VaBank.Jobs.Common
{
    public abstract class EventListenerJob<TContext, TEvent> : ParameterJob<TContext, TEvent>, IEventListenerJob<TEvent>
        where TEvent : IEvent
        where TContext : class, IJobContext<TEvent>
    {
        protected EventListenerJob(ILifetimeScope scope) : base(scope)
        {
        }
    }
}
