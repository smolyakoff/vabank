namespace VaBank.Common.Data.Sorting
{
    public interface ISortableQuery : IQuery
    {
        ISort Sort { get; }

        bool InMemorySorting { get; }
    }
}