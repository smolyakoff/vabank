using System;
using System.Collections.Generic;
using VaBank.Common.Data;
using VaBank.Core.App;
using VaBank.Core.App.Entities;
using VaBank.Core.Maintenance.Entitities;

namespace VaBank.Core.Maintenance.Repositories
{
    public interface IAuditLogRepository
    {
        IList<AuditLogBriefEntry> Query(DbQuery<ApplicationAction> query);

        AuditLogEntry GetAuditEntryDetails(Guid operationId);

        IList<string> GetUniqueCodes();

        void CreateAction(ApplicationAction action);
    }
}
