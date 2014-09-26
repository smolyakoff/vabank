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

            var orderedValues = values.OrderBy<T, T>(x => default(T));

            var methodAsc = typeof(Queryable).GetMethods().Where(x => x.Name == "ThenBy").First();
            var methodDesc = typeof(Queryable).GetMethods().Where(x => x.Name == "ThenByDescending").First();
                        
            foreach (var sorting in descriptor.Sortings)
            {
                var propertyInfo = type.FindProperty(sorting.Property, StringComparison.OrdinalIgnoreCase);
                var selector = typeof(Expressions.ExpressionHelper).GetMethod("BuildLambdaSelector", new[] { typeof(string), typeof(string) })
                    .MakeGenericMethod(type, propertyInfo.PropertyType).Invoke(null, new object[] { "x", propertyInfo.Name });
                
                switch (sorting.Direction)
                {
                    case SortingDirection.Asc:
                        orderedValues = (IOrderedQueryable<T>)methodAsc.MakeGenericMethod(type, propertyInfo.PropertyType)
                            .Invoke(null, new object[] { orderedValues, selector });
                        break;
                    case SortingDirection.Desc:
                        orderedValues = (IOrderedQueryable<T>)methodDesc.MakeGenericMethod(type, propertyInfo.PropertyType)
                            .Invoke(null, new object[] { orderedValues, selector });
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            return orderedValues;
        }
    }
}
