namespace VaBank.Common.Events
{
    public interface IServiceBus : ISendOnlyServiceBus
    {
        void Subscribe<THandler, TEvent>() 
            where THandler : IHandler<TEvent>
            where TEvent : IEvent;
    }
}
