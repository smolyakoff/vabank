using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Core.App;
using VaBank.Core.Common;

namespace VaBank.Core.Maintenance
{
    public class AuditLogBriefEntry : Entity
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
