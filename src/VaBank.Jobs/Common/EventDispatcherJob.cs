using Autofac;
using Autofac.Core;
using VaBank.Common.Events;
using System.Linq;

namespace VaBank.Jobs.Common
{
    public class EventDispatcherJob : ParameterJob<DefaultJobContext<Event>, Event>
    {
        public EventDispatcherJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(DefaultJobContext<Event> context)
        {
            var @event = context.Data;
            var eventType = @event.GetType();
            var handlerType = typeof (IEventListenerJob<>).MakeGenericType(eventType);
            var types = RootScope.ComponentRegistry.Registrations
                .SelectMany(r => r.Services.OfType<IServiceWithType>(), (r, s) => new { r, s })
                .Where(rs => handlerType.IsAssignableFrom(rs.s.ServiceType))
                .Select(rs => rs.r.Activator.LimitType)
                .ToList();
            foreach (var type in types)
            {
                VabankJob.Enqueue(type, @event);
            }
        }
    }
}
