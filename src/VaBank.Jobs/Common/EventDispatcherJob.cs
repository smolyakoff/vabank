using System;
using Autofac;
using Autofac.Core;
using VaBank.Common.Events;
using System.Linq;

namespace VaBank.Jobs.Common
{
    public class EventDispatcherJob : ParameterJob<DefaultJobContext<IEvent>, IEvent>
    {
        public EventDispatcherJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(DefaultJobContext<IEvent> context)
        {
            var @event = context.Data;
            var eventType = @event.GetType();
            var types = RootScope.ComponentRegistry.Registrations
                .SelectMany(r => r.Services.OfType<IServiceWithType>(), (r, s) => new { r, s })
                .Where(rs => IsEventListenerOf(rs.s.ServiceType, eventType))
                .Select(rs => rs.r.Activator.LimitType)
                .Distinct()
                .ToList();
            foreach (var type in types)
            {
                VabankJob.Enqueue(type, @event);
            }
        }

        private static bool IsEventListenerOf(Type serviceType, Type eventType)
        {
            var listenerInterface = serviceType.GetInterfaces()
                    .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof (IEventListenerJob<>));
            if (listenerInterface == null)
            {
                return false;
            }
            var eventArgumentType = listenerInterface.GetGenericArguments()[0];
            return eventArgumentType.IsAssignableFrom(eventType);
        }
    }
}
