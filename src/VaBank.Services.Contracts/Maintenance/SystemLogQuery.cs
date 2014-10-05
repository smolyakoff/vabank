using VaBank.Common.Data.Filtering;

namespace VaBank.Services.Contracts.Maintenance
{
    public class SystemLogQuery : IFilterableQuery, IClientFilterable
    {
        public SystemLogQuery()
        {
            Filter = new AlwaysTrueFilter();
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
