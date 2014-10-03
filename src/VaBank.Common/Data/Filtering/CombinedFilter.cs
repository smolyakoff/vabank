using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using VaBank.Common.Data.Linq;

namespace VaBank.Common.Data.Filtering
{
    public class CombinedFilter : IFilter
    {
        public CombinedFilter()
        {
            Filters = new Collection<IFilter>();
            Logic = FilterLogic.And;
        }

        [JsonProperty(Required = Required.Always)]
        public FilterLogic Logic { get; set; }

        [JsonProperty(Required = Required.Always)]
        public ICollection<IFilter> Filters { get; private set; }

        public bool HasChildren
        {
            get { return Filters.Count > 0; }
        }

        public Expression<Func<T, bool>> ToExpression<T>() where T : class
        {
            if (Filters.Count == 0)
            {
                return x => true;
            }
            if (Filters.Count == 1)
            {
                return Filters.First().ToExpression<T>();
            }
            Expression<Func<T, bool>> expression = Filters.First().ToExpression<T>();
            expression = Filters.ToList()
                .Skip(1)
                .Aggregate(expression, (current, filter) =>
                    Logic == FilterLogic.And
                        ? ExpressionCombiner.And(current, filter.ToExpression<T>())
                        : ExpressionCombiner.Or(current, filter.ToExpression<T>()));
            return expression;
        }
    }
}