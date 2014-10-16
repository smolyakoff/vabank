using VaBank.Common.Events;

namespace VaBank.Jobs.Common
{
    internal interface IEventListenerJob<TEvent> : IJob
        where TEvent : Event
    {
    }
}
