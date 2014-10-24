using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Maintenance;

namespace VaBank.Jobs.Maintenance.Audit
{
    public class AuditJobContext : DefaultJobContext<IAuditedEvent>
    {
        public ILogManagementService LogManagementService { get; set; }
    }
}
