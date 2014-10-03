using System;
using System.Collections.Generic;
using System.Linq;

namespace VaBank.Common.Data.Sorting
{
    public static class SortingExtensions
    {
        private static readonly Dictionary<SortDirection, string> Mapping = new Dictionary<SortDirection, string>
        {
            {SortDirection.Ascending, "ASC"},
            {SortDirection.Descending, "DESC"}
        };

        private static readonly Dictionary<string, SortDirection> ReverseMapping = Mapping.ToDictionary(x => x.Value,
            x => x.Key, StringComparer.OrdinalIgnoreCase);

        public static DelegateSort<T> StronglyTyped<T>(this ISort sort) where T : class
        {
            if (sort == null)
            {
                throw new ArgumentNullException("sort");
            }
            return new DelegateSort<T>(sort.ToDelegate<T>());
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, ISort sort)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException("queryable");
            }
            if (sort == null)
            {
                throw new ArgumentNullException("sort");
            }
            Func<IQueryable<T>, IQueryable<T>> sorter = sort.ToDelegate<T>();
            return sorter(queryable);
        }

        internal static string ToSqlString(this SortDirection sortDirection)
        {
            if (!Mapping.ContainsKey(sortDirection))
            {
                throw new NotSupportedException(string.Format("Sort direction of {0} is not supported.", sortDirection));
            }
            return Mapping[sortDirection];
        }

        internal static SortDirection ToSortDirection(string ascOrDesc)
        {
            if (string.IsNullOrEmpty(ascOrDesc))
            {
                throw new ArgumentNullException("ascOrDesc");
            }
            if (!ReverseMapping.ContainsKey(ascOrDesc))
            {
                throw new NotSupportedException(string.Format("Sort direction of {0} is not supported.", ascOrDesc));
            }
            return ReverseMapping[ascOrDesc];
        }
    }
}