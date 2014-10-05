using System;

namespace VaBank.Services.Contracts.Maintenance
{
    public class SystemLogEntryBriefModel
    {
        public long EventId { get; set; }

        public string Application { get; set; }

        public DateTime TimestampUtc { get; set; }

        public string Level { get; set; }

        public string Type { get; set; }

        public string User { get; set; }

        public string Message { get; set; }
    }
}
