using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Core.App;
using VaBank.Core.App.Entities;

namespace VaBank.Core.Maintenance.Entitities
{
    public class AuditLogEntry : AuditLogBriefEntry
    {
        public AuditLogEntry(Operation operation, IEnumerable<ApplicationAction> appActions,
            IEnumerable<DatabaseAction> dbActions) : base(operation, appActions)
        {
            if (dbActions == null)
                throw new ArgumentNullException("dbActions");
            DatabaseActions = dbActions.ToList();
        }

        //List of db changes
        public List<DatabaseAction> DatabaseActions { get; protected set; }
    }
}
