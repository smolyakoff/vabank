using Newtonsoft.Json;
using VaBank.Common.Events;

namespace VaBank.Jobs
{
    [JsonObject(ItemTypeNameHandling = TypeNameHandling.All)]
    public class ApplicationStartedEvent : Event
    {
        public string Message
        {
            get { return "abcd"; }
        }
    }
}