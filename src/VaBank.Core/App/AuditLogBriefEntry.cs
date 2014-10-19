using System;
using System.Collections.Generic;

namespace VaBank.Core.App
{
    public class AuditLogBriefEntry
    {
        public AuditLogBriefEntry(Operation operation)
        {
            if (operation == null)
                throw new ArgumentNullException("operation");
            Operation = operation;
        }

        public Operation Operation { get; private set; }

        //List of actions
        public List<ApplicationAction> ApplicationActions { get; set; } 
    }
}
