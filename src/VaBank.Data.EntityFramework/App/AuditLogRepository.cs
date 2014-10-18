using System;
using System.Collections.Generic;
using System.Data.Entity;
using VaBank.Common.Data;
using VaBank.Common.Data.Database;
using VaBank.Common.Data.Repositories;
using VaBank.Core.App;

namespace VaBank.Data.EntityFramework.App
{
    public class AuditLogRepository : IRepository, IAuditLogRepository
    {
        private readonly ITransactionProvider _transactionProvider;
        private readonly DbContext _dbContext;

        public AuditLogRepository(DbContext dbContext, ITransactionProvider transactionProvider)
        {
            if (transactionProvider == null)
                throw new ArgumentNullException("transactionProvider");
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            _transactionProvider = transactionProvider;
            _dbContext = dbContext;
        }

        public IList<AuditLogBriefEntry> GetAuditEntries(DbQuery<ApplicationAction> query)
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
            _dbContext.Set<ApplicationAction>().Add(action);
        }
    }
}
