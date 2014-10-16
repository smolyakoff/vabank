using System;
using System.Collections.Generic;
using Autofac;
using VaBank.Common.Events;

namespace VaBank.Jobs.Common
{
    public class HangfireServiceBus : IServiceBus
    {
        private readonly ILifetimeScope _scope;

        public HangfireServiceBus(ILifetimeScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException("scope");
            _scope = scope;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            foreach (var handler in _scope.Resolve<IEnumerable<IEventListener<TEvent>>>())
            {
                handler.Handle(@event);
            }
            /*foreach (var handler in
                from type in _subscribers
                where type.CanHandle<TEvent>()
                select (IHandler<TEvent>)(_scope.IsRegistered(type)
                    ? _scope.Resolve(type)
                    : (!type.IsGenericType
                        ? Activator.CreateInstance(type)
                        : Activator.CreateInstance(type.MakeGenericType(typeof(TEvent))))))
            {
                handler.Handle(appEvent);
            }*/
        }    
    }
}