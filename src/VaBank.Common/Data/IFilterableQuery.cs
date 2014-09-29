using VaBank.Common.Data.Filtering;

namespace VaBank.Common.Data
{
    public interface IFilterableQuery : IQuery
    {
        IFilter Filter { get; set;  }

        bool InMemoryFiltering { get; set; }
    }
}
