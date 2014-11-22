using System;
using Newtonsoft.Json;
using VaBank.Common.Events;
using VaBank.Common.Validation;

namespace VaBank.Services.Contracts.Common.Events
{
    public class PostponedEvent : ApplicationEvent
    {
        public PostponedEvent(IEvent @event, DateTime scheduledDateUtc)
        {
            Argument.NotNull(@event, "event");
            Argument.Satisfies(scheduledDateUtc, x => x > DateTime.UtcNow, "scheduledDateUtc");

            Event = @event;
            ScheduledDateUtc = scheduledDateUtc;
        }

        [JsonConstructor]
        protected PostponedEvent()
        {
        }

        [JsonProperty]
        public IEvent Event { get; private set; }

        [JsonProperty]
        public DateTime ScheduledDateUtc { get; private set; }
    }
}
