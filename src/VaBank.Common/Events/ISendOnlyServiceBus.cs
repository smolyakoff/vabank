namespace VaBank.Common.Events
{
    public interface ISendOnlyServiceBus
    {
        void Publish<TEvent>(TEvent @event) where TEvent : class, IEvent;
    }
}
