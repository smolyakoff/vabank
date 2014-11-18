using VaBank.Common.Data.Repositories;
using VaBank.Core.App.Repositories;
using VaBank.Core.Common;
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

        public IRepository<Transaction> Transactions { get; set; }

        public IHistoricalRepository HistoricalRepository { get; set; }
    }
}
