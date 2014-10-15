using System;

namespace VaBank.Common.Events
{
    public interface IHandler<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent appEvent);
    }
}
