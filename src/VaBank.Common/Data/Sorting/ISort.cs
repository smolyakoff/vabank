using System;
using System.Linq;
using Newtonsoft.Json;
using VaBank.Common.Data.Sorting.Converters;

namespace VaBank.Common.Data.Sorting
{
    [JsonConverter(typeof (JsonSortConverter))]
    public interface ISort
    {
        Func<IQueryable<T>, IQueryable<T>> ToDelegate<T>();
    }
}