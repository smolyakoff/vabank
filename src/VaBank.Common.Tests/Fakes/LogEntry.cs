using System;

namespace VaBank.Common.Tests.Fakes
{
    internal class LogEntry
    {
        public DateTime TimestampUtc { get; set; }

        public string Level { get; set; }

        public string Type { get; set; }
    }
}
