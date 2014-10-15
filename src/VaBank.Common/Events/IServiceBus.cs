namespace VaBank.Common.Events
{
    public interface IServiceBus : ISendOnlyServiceBus
    {
        void Subscribe<TEvent>(IHandler handler);
    }
}
