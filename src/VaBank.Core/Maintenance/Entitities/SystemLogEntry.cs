using System;
using VaBank.Core.Common;

namespace VaBank.Core.Maintenance.Entitities
{
    public class SystemLogEntry : Entity<long>
    {
        public string Application { get; set; }
        public DateTime TimeStampUtc { get; set; }
        public string Level { get; set; }
        public string Type { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Source { get; set; }
    }
}