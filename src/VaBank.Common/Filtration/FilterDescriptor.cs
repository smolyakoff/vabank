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
        private const string ParamName = "o";

        public Filter Context { get; set; }

        public Expression<Func<T, bool>> ToExpression<T>()
        {
            Expression body;

            switch (Context.Type)
            {
                case FilterType.Combiner:
                    body = BuildCombinerFilter<T>((CombinerFilter)Context);
                    break;
                case FilterType.Expression:
                    body = BuildExpressionFilter<T>(((ExpressionFilter)Context));
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return CompleteBuild<T>(body);
        }

        private Expression BuildExpressionFilter<T>(ExpressionFilter filter)
        {
            Expression body, left, right, param;

            var type = typeof(T);
            string propName = null;

            foreach (var property in type.GetProperties())
            {
                if (property.Name.Equals(filter.Property, StringComparison.OrdinalIgnoreCase))
                    propName = property.Name;
            }

            param = Expression.Parameter(type, ParamName);            
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
                    methodInfo = typeof(string).GetRuntimeMethod("Contains", new[] { typeof(string) });
                    body = Expression.Call(right, methodInfo, left);
                    break;

                case FilterOperator.NotIn:
                    methodInfo = typeof(string).GetRuntimeMethod("Contains", new[] { typeof(string) });
                    body = Expression.Not(Expression.Call(right, methodInfo, left));
                    break;

                case FilterOperator.StartsWith:
                    methodInfo = typeof(string).GetRuntimeMethod("StartsWith", new[] { typeof(string) });
                    body = Expression.Call(left, methodInfo, right);
                    break;

                case FilterOperator.NotStartsWith:
                    methodInfo = typeof(string).GetRuntimeMethod("StartsWith", new[] { typeof(string) });
                    body = Expression.Not(Expression.Call(left, methodInfo, right));
                    break;

                case FilterOperator.EndsWith:
                    methodInfo = typeof(string).GetRuntimeMethod("EndsWith", new[] { typeof(string) });
                    body = Expression.Call(left, methodInfo, right);
                    break;

                case FilterOperator.NotEndsWith:
                    methodInfo = typeof(string).GetRuntimeMethod("EndsWith", new[] { typeof(string) });
                    body = Expression.Not(Expression.Call(left, methodInfo, right));
                    break;

                case FilterOperator.Contains:
                    methodInfo = typeof(string).GetRuntimeMethod("Contains", new[] { typeof(string) });
                    body = Expression.Call(left, methodInfo, right);
                    break;

                case FilterOperator.NotContains:
                    methodInfo = typeof(string).GetRuntimeMethod("Contains", new[] { typeof(string) });
                    body = Expression.Not(Expression.Call(left, methodInfo, right));
                    break;

                default:
                    throw new InvalidOperationException();
            }

            return body;
        }

        private Expression BuildCombinerFilter<T>(CombinerFilter filter)
        {
            Expression body = null;
            foreach (var item in filter.Filters)
            {
                Expression expr = null;
                if (item is ExpressionFilter)
                    expr = BuildExpressionFilter<T>((ExpressionFilter)item);
                if (item is CombinerFilter)
                    expr = BuildCombinerFilter<T>((CombinerFilter)item);
                switch (filter.Logic)
                {
                    case FilterLogic.And:
                        body = Expression.And(body, expr);
                        break;
                    case FilterLogic.Or:
                        body = Expression.Or(body, expr);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            return body;
        }

        private Expression<Func<T, bool>> CompleteBuild<T>(Expression body)
        {
            return Expression.Lambda<Func<T, bool>>(body, Expression.Parameter(typeof(T), ParamName));
        }
    }
}
