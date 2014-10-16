using System;
using VaBank.Common.Events;

namespace VaBank.Services.Contracts.Events
{
    public  abstract class ApplicationEvent : Event
    {
        protected ApplicationEvent()
        {
            DateUtc = DateTime.UtcNow;
        }

        public DateTime DateUtc { get; protected set; }
    }
}
