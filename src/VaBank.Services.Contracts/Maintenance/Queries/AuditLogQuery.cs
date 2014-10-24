using VaBank.Common.Data.Filtering;

namespace VaBank.Services.Contracts.Maintenance.Queries
{
    public class AuditLogQuery : IClientFilterable
    {
        public AuditLogQuery()
        {
            ClientFilter = new AlwaysTrueFilter();
        }
        public IFilter ClientFilter { get; set; }
    }
}
