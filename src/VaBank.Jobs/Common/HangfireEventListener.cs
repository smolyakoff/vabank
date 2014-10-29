using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Events;

namespace VaBank.Jobs.Common
{
    public class HangfireEventListener : IEventListener<IEvent>
    {
        private static readonly Dictionary<Type, List<Type>> HandlersCache = new Dictionary<Type, List<Type>>();

        protected readonly ILifetimeScope Scope;

        public HangfireEventListener(ILifetimeScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException("scope");
            Scope = scope;
        }
        
        public void Handle(IEvent appEvent)
        {
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
                VabankJob.Enqueue(type, appEvent);
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
