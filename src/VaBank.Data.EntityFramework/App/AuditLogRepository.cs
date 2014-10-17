using System;
using System.Collections.Generic;
using VaBank.Common.Data.Repositories;
using VaBank.Core.App;

namespace VaBank.Data.EntityFramework.App
{
    public class AuditLogRepository : IRepository, IAuditLogRepository
    {
        public IList<AuditLogBriefEntry> GetAuditEntries(VaBank.Common.Data.DbQuery<ApplicationAction> query)
        {
            throw new NotImplementedException();
        }

        public AuditLogEntry GetAuditEntryDetails(Guid operationId)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetUniqueCodes()
        {
            throw new NotImplementedException();
        }

        public void CreateAction(ApplicationAction action)
        {
            throw new NotImplementedException();
        }
    }
}
