using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using VaBank.Common.Events;
using VaBank.Services.Contracts.Common.Events;

namespace VaBank.Jobs.Common
{
    public class HangfireEventListener : IEventListener<IEvent>
    {
        private static readonly Dictionary<Type, List<Type>> HandlersCache = new Dictionary<Type, List<Type>>();

        protected readonly ILifetimeScope Scope;

        private readonly Logger _logger;

        public HangfireEventListener(ILifetimeScope scope)
        {
            if (scope == null)
            {
                throw new ArgumentNullException("scope");
            }
            _logger = LogManager.GetCurrentClassLogger();
            Scope = scope;
        }
        
        public void Handle(IEvent appEvent)
        {
            Action<Type, IEvent> enqueue = (type, @event) => VabankJob.Enqueue(type, @event);
            var postponed = appEvent as PostponedEvent;
            if (postponed != null)
            {
                _logger.Info("Event of type [{0}] was postponed to {1}.", postponed.Event.GetType().Name, postponed.ScheduledDateUtc);
                appEvent = postponed.Event;
                var delay = postponed.ScheduledDateUtc - DateTime.UtcNow;
                enqueue = (type, @event) => VabankJob.Schedule(type, @event, delay);
            }
            var eventType = appEvent.GetType();
            IEnumerable<Type> handlerTypes;
            if (HandlersCache.ContainsKey(eventType))
            {
                handlerTypes = HandlersCache[eventType];
            }
            else
            {
                handlerTypes = Scope.ComponentRegistry.Registrations
                .SelectMany(r => r.Services.OfType<IServiceWithType>(), (r, s) => new { r, s })
                .Where(rs => IsEventListenerOf(rs.s.ServiceType, eventType))
                .Select(rs => rs.r.Activator.LimitType)
                .Distinct()
                .ToList();
            }
            
            foreach (var type in handlerTypes)
            {
                enqueue(type, appEvent);
            }
        }

        private static bool IsEventListenerOf(Type serviceType, Type eventType)
        {
            var listenerInterface = serviceType.GetInterfaces()
                    .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEventListenerJob<>));
            if (listenerInterface == null)
            {
                return false;
            }
            var eventArgumentType = listenerInterface.GetGenericArguments()[0];
            return eventArgumentType.IsAssignableFrom(eventType);
        }
    }
}
