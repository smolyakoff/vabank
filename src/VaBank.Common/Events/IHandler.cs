using System;

namespace VaBank.Common.Events
{
    public interface IHandler
    {
        void Handle();
        bool CanHandle<TEvent>(TEvent appEvent) where TEvent : IEvent;
        bool CanHandle(Type type);
    }
}
