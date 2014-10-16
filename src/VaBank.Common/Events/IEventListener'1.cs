namespace VaBank.Common.Events
{
    public interface IEventListener<in TEvent> where TEvent : Event
    {
        void Handle(TEvent appEvent);
    }
}
