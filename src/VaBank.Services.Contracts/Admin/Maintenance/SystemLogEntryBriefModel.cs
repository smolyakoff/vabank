using System;

namespace VaBank.Services.Contracts.Admin.Maintenance
{
    public class SystemLogEntryBriefModel
    {
        public long EventId { get; set; }

        public string Application { get; set; }

        public DateTime TimestampUtc { get; set; }

        public SystemLogLevelModel Level { get; set; }

        public string Type { get; set; }

        public string User { get; set; }

        public string Message { get; set; }
    }
}
