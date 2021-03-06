﻿using VaBank.Common.Data.Repositories;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.App.Repositories;
using VaBank.Core.Common.History;
using VaBank.Core.Maintenance.Entitities;
using VaBank.Core.Maintenance.Repositories;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;

namespace VaBank.Services.Maintenance
{
    public class MaintenanceRepositories : IDependencyCollection
    {
        public IQueryRepository<SystemLogEntry> LogEntries { get; set; }

        public IAuditLogRepository AuditLogs { get; set; }

        public IOperationRepository Operations { get; set; }

        public IQueryRepository<Transaction> Transactions { get; set; }

        public IHistoricalRepository HistoricalRepository { get; set; }

        public IRepository<Account> Accounts { get; set; }
    }
}
