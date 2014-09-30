using VaBank.Common.Data.Filtering;

namespace VaBank.Common.Data
{
    public interface IFilterableQuery : IQuery
    {
        IFilter Filter { get; }

        bool InMemoryFiltering { get; }
    }
}
