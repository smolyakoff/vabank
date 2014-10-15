namespace VaBank.Common.Events
{
    public interface ISendOnlyServiceBus
    {
        void Publish<TEvent>(TEvent appEvent) where TEvent : IEvent;
    }
}
