namespace VaBank.Common.Events
{
    public interface IServiceBus : ISendOnlyServiceBus
    {
        void Subscribe<TEvent>(IEventListener<TEvent> eventListener) where TEvent : Event;
    }
}
