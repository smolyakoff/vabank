using System;
using System.Collections.Generic;
using System.Linq;

namespace VaBank.Common.Events
{
    public class InMemoryServiceBus : IServiceBus
    {
        private readonly SynchronizedCollection<object> _listeners = new SynchronizedCollection<object>();

        public void Publish<TEvent>(TEvent @event) where TEvent : Event
        {
            if (@event == null)
            {
                throw new ArgumentNullException("@event");
            }
            foreach (var listener in _listeners
                .Cast<dynamic>()
                .Where(listener => CanHandle<TEvent>(listener)))
            {
                listener.Handle(@event);
            }
        }

        public void Subscribe<TEvent>(IEventListener<TEvent> eventListener) 
            where TEvent : Event
        {
            if (eventListener == null)
            {
                throw new ArgumentNullException("eventListener");
            }
            _listeners.Add(eventListener);
        }

        private static bool CanHandle<TEvent>(dynamic listener)
            where TEvent : Event
        {
            return listener is IEventListener<TEvent>;
        }
    }
}
