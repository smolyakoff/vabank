using Newtonsoft.Json;

namespace VaBank.Jobs.Common.Settings
{
    public class RecurringJobSettings
    {
        [JsonProperty]
        public string Cron { get; protected set; }

        [JsonProperty]
        public bool Enabled { get; protected set; }
    }
}
