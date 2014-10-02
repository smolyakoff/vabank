using VaBank.Common.Data.Sorting;

namespace VaBank.Common.Data.Paging
{
    public interface IPageableQuery : ISortableQuery
    {
        int PageSize { get; }

        int PageNumber { get; }

        bool InMemoryPaging { get; }
    }
}
