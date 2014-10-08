using System;
using VaBank.Common.Data.Paging;

namespace VaBank.Common.Data
{
    public class PagedDbQuery<T> : DbQuery<T>, IPageableQuery
        where T : class
    {
        private bool _inMemoryPaging = false;

        private int _pageSize;

        public PagedDbQuery(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "Page number should be greater than 1.");
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize", pageNumber, "Page size should be greater than 1.");
            }
            PageNumber = pageNumber;
            PageSize = pageSize;
        } 

        public override DbQuery<T> FromClientQuery(IClientQuery clientQuery)
        {
            var pageable = clientQuery as IClientPageable;
            if (pageable != null)
            {
                var page = pageable.ClientPage;
                if (page.PageNumber.HasValue && page.PageNumber.Value < 1)
                {
                    throw new ArgumentOutOfRangeException("pageNumber", page.PageNumber.Value,
                        "Page number should be greater than 1.");
                }
                if (page.PageNumber.HasValue)
                {
                    PageNumber = page.PageNumber.Value;
                }
                if (page.PageSize.HasValue && page.PageSize.Value < 1)
                {
                    throw new ArgumentOutOfRangeException("pageSize", page.PageSize.Value,
                        "Page size should be greater than 1.");
                }
                if (page.PageSize.HasValue)
                {
                    PageSize = page.PageSize.Value;
                }
            }
            return base.FromClientQuery(clientQuery);
        }

        public PagedDbQuery<T> WithPaging(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "Page number should be greater than 1.");
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize", pageNumber, "Page size should be greater than 1.");
            }
            PageNumber = pageNumber;
            PageSize = pageSize;
            return this;
        } 

        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public bool InMemoryPaging
        {
            get { return _inMemoryPaging; }
        }
    }
}
