namespace VaBank.Common.Data.Filtering
{
    public interface IFilterableQuery : IQuery
    {
        IFilter Filter { get; }

        bool InMemoryFiltering { get; }
    }
}
