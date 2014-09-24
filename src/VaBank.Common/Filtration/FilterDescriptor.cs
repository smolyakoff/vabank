using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Common.Filtration
{
    public class FilterDescriptor
    {        
        public Filter Context { get; set; }

        public Expression<Func<T, bool>> ToExpression<T>()
        {
            Expression<Func<T, bool>> expr;

            switch (Context.Type)
            {
                case FilterType.Combiner:
                    expr = BuildCombinerFilter((CombinerFilter)Context);
                    break;
                case FilterType.Expression:
                    expr = BuildExpressionFilter(((ExpressionFilter)Context));
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return expr;
        }

        private Expression<Func<T, bool>> BuildExpressionFilter(ExpressionFilter filter)
        {
            Expression body, left, right, param;

            var type = typeof(T);
            string propName = null;

            foreach (var property in type.GetProperties())
            {
                if (property.Name.Equals(filter.Property, StringComparison.OrdinalIgnoreCase))
                    propName = property.Name;
            }

            param = Expression.Parameter(type, "f");            
            left = Expression.Property(param, propName);
            right = Expression.Constant(filter.Value);

            MethodInfo methodInfo = null;

            switch (filter.Operator)
            {
                case FilterOperator.Equality:
                    body = Expression.Equal(left, right);
                    break;

                case FilterOperator.Inequality:
                    body = Expression.NotEqual(left, right);
                    break;

                case FilterOperator.GreaterThan:
                    body = Expression.GreaterThan(left, right);
                    break;

                case FilterOperator.GreaterThanOrEqual:
                    body = Expression.GreaterThanOrEqual(left, right);
                    break;

                case FilterOperator.LessThan:
                    body = Expression.LessThan(left, right);
                    break;

                case FilterOperator.LessThanOrEqual:
                    body = Expression.LessThanOrEqual(left, right);
                    break;

                case FilterOperator.In:
                    methodInfo = typeof(string).GetRuntimeMethod("Contains", typeof(string));
                    body = Expression.Call(right, methodInfo, left);
                    break;

                case FilterOperator.NotIn:
                    methodInfo = typeof(string).GetRuntimeMethod("Contains", typeof(string));
                    body = Expression.Not(Expression.Call(right, methodInfo, left));
                    break;

                case FilterOperator.StartsWith:
                    methodInfo = typeof(string).GetRuntimeMethod("StartsWith", typeof(string));
                    body = Expression.Call(left, methodInfo, right);
                    break;

                case FilterOperator.NotStartsWith:
                    methodInfo = typeof(string).GetRuntimeMethod("StartsWith", typeof(string));
                    body = Expression.Not(Expression.Call(left, methodInfo, right));
                    break;

                case FilterOperator.EndsWith:
                    methodInfo = typeof(string).GetRuntimeMethod("EndsWith", typeof(string));
                    body = Expression.Call(left, methodInfo, right);
                    break;

                case FilterOperator.NotEndsWith:
                    methodInfo = typeof(string).GetRuntimeMethod("EndsWith", typeof(string));
                    body = Expression.Not(Expression.Call(left, methodInfo, right));
                    break;

                case FilterOperator.Contains:
                    methodInfo = typeof(string).GetRuntimeMethod("Contains", typeof(string));
                    body = Expression.Call(left, methodInfo, right);
                    break;

                case FilterOperator.NotContains:
                    methodInfo = typeof(string).GetRuntimeMethod("Contains", typeof(string));
                    body = Expression.Not(Expression.Call(left, methodInfo, right));
                    break;

                default:
                    throw new InvalidOperationException();
            }
            
            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        private Expression<Func<T, bool>> BuildCombinerFilter(CombinerFilter filter)
        {

        }
    }
}
