using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace VaBank.Common.Data.Sorting
{
    public class MultiSort : ISort
    {
        [JsonProperty("Sorts")] private readonly List<SimpleSort> _sorts;

        private MultiSort()
        {
            _sorts = new List<SimpleSort>();
        }

        [JsonIgnore]
        public IEnumerable<SimpleSort> Sorts
        {
            get { return _sorts.ToList(); }
        }

        public Func<IQueryable<T>, IQueryable<T>> ToDelegate<T>()
        {
            string expression = string.Join(", ", _sorts.Select(x => x.ToSqlExpression()));
            var linqSort = new DynamicLinqSort(expression);
            return linqSort.ToDelegate<T>();
        }
    }
}