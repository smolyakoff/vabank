using System;
using System.Collections.Generic;
using System.Linq;

namespace VaBank.Core.App
{
    public class AuditLogBriefEntry
    {
        public AuditLogBriefEntry(Operation operation, IEnumerable<ApplicationAction> actions)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }
            if (actions == null)
            {
                throw new ArgumentNullException("actions");
            }
            Operation = operation;
            ApplicationActions = actions.ToList();
        }

        public Operation Operation { get; private set; }

        public List<ApplicationAction> ApplicationActions { get; private set; } 
    }
}
