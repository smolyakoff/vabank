using System.Collections.Concurrent;
using VaBank.Common.Events;
using System;
using Autofac;
using System.Linq;

namespace VaBank.UI.Web.Api.Infrastructure.Events
{
    public class HangfireServiceBus : IServiceBus
    {
        private readonly BlockingCollection<Type> _subscribers;
        private readonly ILifetimeScope _scope;

        public HangfireServiceBus(ILifetimeScope scope, params Type[] handlers)
        {
            if (scope == null)
                throw new ArgumentNullException("scope");
            if (handlers != null && handles.Any(x => !x.IsHandler()))
                throw new InvalidCastException();
            _scope = scope;
            _subscribers = new BlockingCollection<Type>();
            
            if (handlers != null)
                foreach (var handler in handlers)
                {
                    _subscribers.Add(handler);
                }
        }

        public void Publish<TEvent>(TEvent appEvent) where TEvent : IEvent
        {
            foreach (var item in _subscribers.Where(x => x.CanHandle<TEvent>()))
            {
                var handler = _scope.Resolve(item);
                if (handler != null)
                    ((IHandler<TEvent>)handler).Handle(appEvent);
            }
        }

        public void Subscribe<THandler, TEvent>()
            where THandler : IHandler<TEvent>
            where TEvent : IEvent
        {            
            _subscribers.Add(typeof(THandler));            
        }        
    }
}