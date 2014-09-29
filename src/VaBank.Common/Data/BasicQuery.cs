using System.ComponentModel;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Filtering.Converters;

namespace VaBank.Common.Data
{
    public class BasicQuery : IFilterableQuery
    {
        public BasicQuery()
        {
            Filter = new EmptyFilter();
            InMemoryFiltering = false;
        }

        public IFilter Filter { get; set; }

        public bool InMemoryFiltering { get; set; }
    }
}
