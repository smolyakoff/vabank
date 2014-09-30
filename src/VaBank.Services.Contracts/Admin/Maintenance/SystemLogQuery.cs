using VaBank.Common.Data;
using VaBank.Common.Data.Filtering;

namespace VaBank.Services.Contracts.Admin.Maintenance
{
    public class SystemLogQuery : IClientFilterableQuery
    {
        public SystemLogQuery()
        {
            Filter = new EmptyFilter();
        }

        public void ApplyFilter(IFilter filter)
        {
            Filter = filter;
        }

        public IFilter Filter { get; private set; }

        public bool InMemoryFiltering
        {
            get { return false; }
        }
    }
}
