using System;
using System.Collections.Generic;
using System.Linq;

namespace VaBank.Common.Data.Sorting
{
    public static class SortingExtensions
    {
        private static readonly Dictionary<SortDirection, string> Mapping = new Dictionary<SortDirection, string>()
        {
            {SortDirection.Ascending, "ASC"},
            {SortDirection.Descending, "DESC"}
        };

        private static readonly Dictionary<string, SortDirection> ReverseMapping = Mapping.ToDictionary(x => x.Value,
            x => x.Key, StringComparer.OrdinalIgnoreCase);

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
