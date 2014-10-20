using System;
using System.Collections.Generic;
using VaBank.Common.Data;

namespace VaBank.Core.App
{
    public interface IAuditLogRepository
    {
        IList<AuditLogBriefEntry> GetAuditEntries(DbQuery<ApplicationAction> query);

        AuditLogEntry GetAuditEntryDetails(Guid operationId);

        IList<string> GetUniqueCodes();

        void CreateAction(ApplicationAction action);
    }
}
