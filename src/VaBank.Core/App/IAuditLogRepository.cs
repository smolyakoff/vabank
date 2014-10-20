using System;
using System.Collections.Generic;
using VaBank.Common.Data;

namespace VaBank.Core.App
{
    public interface IAuditLogRepository
    {
        //if userIds is null or empty - return for all users
        //note for implementor: should group by operation id
        IList<AuditLogBriefEntry> GetAuditEntries(DbQuery<ApplicationAction> query);

        AuditLogEntry GetAuditEntryDetails(Guid operationId);

        IList<string> GetUniqueCodes();
    }
}
