using VaBank.Common.Data.Repositories;
using VaBank.Core.App;
using VaBank.Core.Maintenance;
using VaBank.Core.Membership;
using VaBank.Services.Common;

namespace VaBank.Services.Maintenance
{
    public class MaintenanceRepositories : IDependencyCollection
    {
        public IQueryRepository<SystemLogEntry> LogEntries { get; set; }

        public IAuditLogRepository AuditLogs { get; set; }

        public IQueryRepository<User> Users { get; set; }
    }
}
