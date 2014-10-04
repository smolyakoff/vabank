using VaBank.Common.Data.Sorting;

namespace VaBank.Common.Data.Paging
{
    internal class NoPagingQuery : IPageableQuery
    {
        public ISort Sort
        {
            get { return new RandomSort(); }
        }

        public bool InMemorySorting
        {
            get { return false; }
        }

        public int PageSize
        {
            get { return int.MaxValue; }
        }

        public int PageNumber
        {
            get { return 1; }
        }

        public bool InMemoryPaging
        {
            get { return false; }
        }
    }
}