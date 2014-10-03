using System;
using System.Linq;
using Newtonsoft.Json;

namespace VaBank.Common.Data.Sorting
{
    public class SimpleSort : ISort
    {
        private SimpleSort()
        {
        }

        [JsonProperty(Required = Required.Always)]
        public string PropertyName { get; private set; }

        [JsonProperty(Required = Required.Always)]
        public SortDirection SortDirection { get; private set; }

        public Func<IQueryable<T>, IQueryable<T>> ToDelegate<T>()
        {
            var linqSort = new DynamicLinqSort(ToSqlExpression());
            return linqSort.ToDelegate<T>();
        }

        public string ToSqlExpression()
        {
            return string.Format("{0} {1}", PropertyName, SortDirection.ToSqlString());
        }

        public override string ToString()
        {
            return ToSqlExpression();
        }
    }
}