using System.Collections.Generic;

namespace VaBank.Core.App
{
    public class AuditLogEntry : AuditLogBriefEntry
    {
        public AuditLogEntry(Operation operation) : base(operation)
        {
        }

        //List of db changes
        public List<DatabaseAction> DatabaseActions { get; set; }
    }
}
