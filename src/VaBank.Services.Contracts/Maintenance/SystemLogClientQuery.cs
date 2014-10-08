using VaBank.Common.Data.Filtering;

namespace VaBank.Services.Contracts.Maintenance
{
    public class SystemLogClientQuery : IClientFilterable
    {
        public SystemLogClientQuery()
        {
            ClientFilter = new AlwaysTrueFilter();
        }

        public IFilter ClientFilter { get; set; }
    }
}
