using System;
using System.Linq.Expressions;
using Newtonsoft.Json;
using VaBank.Common.Data.Filtering.Converters;

namespace VaBank.Common.Data.Filtering
{
    [JsonConverter(typeof (JsonFilterConverter))]
    public interface IFilter
    {
        Expression<Func<T, bool>> ToExpression<T>() where T : class;
    }
}