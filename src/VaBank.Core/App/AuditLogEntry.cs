using System.Collections.Generic;

namespace VaBank.Core.App
{
    public class AuditLogEntry : AuditLogBriefEntry
    {
        //List of db changes
        public List<DatabaseAction> DatabaseActions { get; set; }
    }
}
