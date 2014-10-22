using System;
using System.Collections.Generic;
using VaBank.Common.Data;
using VaBank.Core.App;

namespace VaBank.Core.Maintenance
{
    public interface IAuditLogRepository
    {
        IList<AuditLogBriefEntry> Query(DbQuery<ApplicationAction> query);

        AuditLogEntry GetAuditEntryDetails(Guid operationId);

        IList<string> GetUniqueCodes();

        void CreateAction(ApplicationAction action);
    }
}
