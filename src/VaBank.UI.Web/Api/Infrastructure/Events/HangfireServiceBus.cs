using System.Collections.Concurrent;
using System.Linq;
using Hangfire;
using VaBank.Common.Events;

namespace VaBank.UI.Web.Api.Infrastructure.Events
{
    public class HangfireServiceBus : IServiceBus
    {
        private readonly BlockingCollection<IHandler> _handlers;

        public HangfireServiceBus()
        {
           _handlers = new BlockingCollection<IHandler>();
        }

        public void Publish<TEvent>(TEvent appEvent) where TEvent : IEvent
        {
            foreach (var handler in _handlers.GetConsumingEnumerable()
                .Where(handler => handler.CanHandle(appEvent)))
            {
                var handler1 = handler;
                BackgroundJob.Enqueue(() => handler1.Handle());
            }
        }

        public void Subscribe<TEvent>(IHandler handler)
        {
            _handlers.Add(handler);
        }
    }
}