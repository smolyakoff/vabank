using VaBank.Common.Data;
using VaBank.Common.Data.Filtering;

namespace VaBank.Services.Contracts.Admin.Maintenance
{
    public class SystemLogQuery : IFilterableQuery
    {
        public SystemLogQuery()
        {
            Filter = new EmptyFilter();
        }

        public IFilter Filter { get; set; }
        public bool InMemoryFiltering { get; set; }
    }
}
