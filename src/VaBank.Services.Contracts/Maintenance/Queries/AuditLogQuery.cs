using VaBank.Common.Data.Filtering;

namespace VaBank.Services.Contracts.Maintenance.Queries
{
    public class AuditLogQuery : IClientFilterable
    {
        public AuditLogQuery()
        {
            ClientFilter = new AlwaysTrueFilter();
        }

        /// <summary>
        /// Query for ApplicationAction
        /// </summary>
        public IFilter ClientFilter { get; set; }
    }
}
