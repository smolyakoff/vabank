namespace VaBank.Common.Events
{
    public interface IEventListener<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent appEvent);
    }
}
