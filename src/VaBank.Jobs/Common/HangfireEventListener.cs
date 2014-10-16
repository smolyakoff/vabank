using VaBank.Common.Events;

namespace VaBank.Jobs.Common
{
    public class HangfireEventListener : IEventListener<IEvent>
    {
        public void Handle(IEvent appEvent)
        {
            VabankJob.Enqueue<EventDispatcherJob, IEvent>(appEvent);
        }
    }
}
