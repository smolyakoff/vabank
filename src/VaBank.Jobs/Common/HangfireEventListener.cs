using VaBank.Common.Events;

namespace VaBank.Jobs.Common
{
    public class HangfireEventListener : IEventListener<Event>
    {
        public void Handle(Event appEvent)
        {
            VabankJob.Enqueue<EventDispatcherJob, Event>(appEvent);
        }
    }
}
