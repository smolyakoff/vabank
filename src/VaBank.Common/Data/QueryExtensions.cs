using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Paging;
using VaBank.Common.Data.Sorting;

namespace VaBank.Common.Data
{
    public static class QueryExtensions
    {
        public static DbQuery<T> ToDbQuery<T>(this IClientQuery clientQuery)
            where T : class
        {
            if (clientQuery == null)
            {
                throw new ArgumentNullException("clientQuery");
            }
            var pageable = clientQuery as IClientPageable;
            return pageable != null 
                ? DbQuery.PagedFor<T>().FromClientQuery(clientQuery) 
                : DbQuery.For<T>().FromClientQuery(clientQuery);
        } 


        public static IPagedList<T> QueryPage<T>(this IQueryable<T> queryable, IQuery query)
            where T : class
        {
            if (queryable == null)
            {
                throw new ArgumentNullException("queryable");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            IQueryable<T> filteredSorted = FilterAndSort(queryable, query);
            IPagedList<T> paged = Paging(filteredSorted, query);
            return paged;
        }

        public static IQueryable<T> Query<T>(this IQueryable<T> queryable, IQuery query)
            where T : class
        {
            if (queryable == null)
            {
                throw new ArgumentNullException("queryable");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            IQueryable<T> filteredSorted = FilterAndSort(queryable, query);
            IPagedList<T> paged = Paging(filteredSorted, query);
            return paged.AsQueryable();
        }

        private static IPagedList<T> Paging<T>(this IQueryable<T> sortedQueryable, IQuery query)
            where T : class
        {
            var pageable = query as IPageableQuery;
            Func<IQueryable<T>, IPageableQuery, IPagedList<T>> pager;
            if (pageable == null || pageable is NoPagingQuery)
            {
                pager = (x, f) => new StaticPagedList<T>(x, 1, int.MaxValue, int.MaxValue);
            }
            else if (pageable.InMemoryPaging)
            {
                pager = PageEnumerable;
            }
            else
            {
                pager = PageQueryable;
            }
            IPagedList<T> paged = pager(sortedQueryable, pageable);
            return paged;
        }

        private static IQueryable<T> FilterAndSort<T>(this IQueryable<T> queryable, IQuery query)
            where T : class
        {
            var filterable = query as IFilterableQuery;
            var sortable = query as ISortableQuery;

            Func<IQueryable<T>, IFilter, IQueryable<T>> filterer;
            Func<IQueryable<T>, ISort, IQueryable<T>> sorterer;
            if (filterable == null)
            {
                filterer = (x, f) => x;
            }
            else if (filterable.InMemoryFiltering)
            {
                filterer = FilterEnumerable;
            }
            else
            {
                filterer = FilterQueryable;
            }
            if (sortable == null)
            {
                sorterer = (x, f) => x;
            }
            else if (sortable.InMemorySorting || filterer == FilterEnumerable)
            {
                sorterer = SortEnumerable;
            }
            else
            {
                sorterer = SortQueryable;
            }

            IFilter filter = filterable == null ? new AlwaysTrueFilter() : filterable.Filter;
            ISort sort = sortable == null ? new RandomSort() : sortable.Sort;
            IQueryable<T> filtered = filterer(queryable, filter);
            IQueryable<T> sorted = sorterer(filtered, sort);
            return sorted;
        }

        private static IQueryable<T> FilterQueryable<T>(IQueryable<T> queryable, IFilter filter)
            where T : class
        {
            filter = filter ?? new AlwaysTrueFilter();
            return queryable.Where(filter);
        }

        private static IQueryable<T> FilterEnumerable<T>(IEnumerable<T> enumerable, IFilter filter)
            where T : class
        {
            filter = filter ?? new AlwaysTrueFilter();
            return enumerable.Where(filter.ToExpression<T>().Compile()).AsQueryable();
        }

        private static IQueryable<T> SortQueryable<T>(IQueryable<T> queryable, ISort sort)
            where T : class
        {
            sort = sort ?? new RandomSort();
            return queryable.OrderBy(sort);
        }

        private static IQueryable<T> SortEnumerable<T>(IEnumerable<T> enumerable, ISort sort)
            where T : class
        {
            sort = sort ?? new RandomSort();
            return enumerable.AsQueryable().OrderBy(sort);
        }

        private static IPagedList<T> PageQueryable<T>(IQueryable<T> queryable, IPageableQuery pageable)
            where T : class
        {
            if (pageable.PageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("pageSize", pageable.PageSize,
                    "Page size should be greater than zero.");
            }
            if (pageable.PageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", pageable.PageNumber,
                    "Page number should be greater than zero.");
            }
            return queryable.ToPagedList(pageable.PageNumber, pageable.PageSize);
        }

        private static IPagedList<T> PageEnumerable<T>(IEnumerable<T> enumerable, IPageableQuery pageable)
            where T : class
        {
            if (pageable.PageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("pageSize", pageable.PageSize,
                    "Page size should be greater than zero.");
            }
            if (pageable.PageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", pageable.PageNumber,
                    "Page number should be greater than zero.");
            }
            return new PagedList<T>(enumerable, pageable.PageNumber, pageable.PageSize);
        }
    }
}