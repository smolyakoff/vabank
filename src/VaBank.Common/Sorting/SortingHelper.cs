using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VaBank.Common.Reflection;

namespace VaBank.Common.Sorting
{
    public static class SortingHelper
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> values, SortingDescriptor descriptor)
        {
            var type = typeof(T);

            values = values.OrderBy<T, T>(x => x);
            
            foreach (var sorting in descriptor.Sortings)
            {
                var propertyInfo = type.FindProperty(sorting.Property, StringComparison.OrdinalIgnoreCase);
                var selector = Expressions.ExpressionHelper.BuildLambdaSelector<T, object>("x", propertyInfo.Name);
                
                switch (sorting.Direction)
                {
                    case SortingDirection.Asc:
                        ((IOrderedQueryable<T>)values).ThenBy(selector);
                        break;
                    case SortingDirection.Desc:
                        ((IOrderedQueryable<T>)values).ThenByDescending(selector);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            return values;
        }
    }
}
