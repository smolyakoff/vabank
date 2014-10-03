using VaBank.Common.Data.Sorting;

namespace VaBank.Common.Data.Paging
{
    public interface IPageableQuery : ISortableQuery, IPage
    {
        bool InMemoryPaging { get; }
    }
}