using VaBank.Jobs.Common;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Maintenance;

namespace VaBank.Jobs.Maintenance
{
    public class AuditJobContext : DefaultJobContext<IAuditedEvent>
    {
        public ILogService LogService { get; set; }
    }
}
