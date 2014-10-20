using System.Collections.Generic;

namespace VaBank.Core.App
{
    public class AuditLogBriefEntry
    {
        public Operation Operation { get; private set; }

        //List of actions
        public List<ApplicationAction> ApplicationActions { get; set; } 
    }
}
