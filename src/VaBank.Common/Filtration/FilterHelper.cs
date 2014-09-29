using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Reflection;

namespace VaBank.Common.Filtration
{
    public static class FilterHelper
    {
        private const string ParamName = "x";

        public static Expression<Func<T, bool>> ToExpression<T>(this FilterDescriptor descriptor)
        {
            Expression body;

            switch (descriptor.Context.Type)
            {
                case FilterType.Combiner:
                    body = BuildCombinerFilter<T>((CombinerFilter)descriptor.Context);
                    break;
                case FilterType.Expression:
                    body = BuildExpressionFilter<T>(((ExpressionFilter)descriptor.Context));
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return CompleteBuild<T>(body);
        }

        private static Expression BuildExpressionFilter<T>(ExpressionFilter filter)
        {
            Expression body, left, right, param;

            var type = typeof(T);
            var propInfo = type.FindProperty(filter.Property, StringComparison.OrdinalIgnoreCase);

            if (propInfo == null)
                throw new InvalidOperationException();

            param = Expression.Parameter(type, ParamName);
            left = Expression.Property(param, propInfo.Name);

            if (propInfo.PropertyType != filter.Value.GetType())
            {
                var converter = TypeDescriptor.GetConverter(propInfo.PropertyType);
                right = Expression.Constant(converter.ConvertFrom(filter.Value));
            }
            else
                right = Expression.Constant(filter.Value);

            MethodInfo methodInfo = null;

            switch (filter.Operator)
            {
                case FilterOperator.Equal:
                    body = Expression.Equal(left, right);
                    break;

                case FilterOperator.NotEqual:
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

        private static Expression BuildCombinerFilter<T>(CombinerFilter filter)
        {
            Expression body = null;
            var filters = filter.Filters.ToList();

            if (filters[0] is ExpressionFilter)
                body = BuildExpressionFilter<T>((ExpressionFilter)filters[0]);
            if (filters[0] is CombinerFilter)
                body = BuildCombinerFilter<T>((CombinerFilter)filters[0]);

            for (int i = 1; i < filters.Count; i++)
            {
                Expression expr = null;
                if (filters[i] is ExpressionFilter)
                    expr = BuildExpressionFilter<T>((ExpressionFilter)filters[i]);
                if (filters[i] is CombinerFilter)
                    expr = BuildCombinerFilter<T>((CombinerFilter)filters[i]);
                switch (filter.Logic)
                {
                    case FilterLogic.And:
                        body = Expression.AndAlso(body, expr);
                        break;
                    case FilterLogic.Or:
                        body = Expression.OrElse(body, expr);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            return body;
        }

        private static Expression<Func<T, bool>> CompleteBuild<T>(Expression body)
        {
            var lambda = Expression.Lambda<Func<T, bool>>(body, Expression.Parameter(typeof(T), ParamName));
            if (lambda.CanReduce)
                lambda = (Expression<Func<T, bool>>)lambda.Reduce();
            return lambda;
        }

        public static FilterType ToFilterType(this string value)
        {
            var str = value.ToLower();
            switch (str)
            {
                case "expression":
                    return FilterType.Expression;
                case "combiner":
                    return FilterType.Combiner;
                default:
                    throw new InvalidCastException();
            }
        }

        public static FilterLogic ToFilterLogic(this string value)
        {
            var str = value.ToLower();
            switch (str)
            {
                case "and":
                    return FilterLogic.And;
                case "or":
                    return FilterLogic.Or;
                default:
                    throw new InvalidCastException();
            }
        }

        public static FilterOperator ToFilterOperator(this string value)
        {
            var str = value.ToLower();
            switch (str)
            {
                case "==":
                    return FilterOperator.Equal;
                case "!=":
                    return FilterOperator.NotEqual;
                case "<":
                    return FilterOperator.LessThan;
                case "<=":
                    return FilterOperator.LessThanOrEqual;
                case ">":
                    return FilterOperator.GreaterThan;
                case ">=":
                    return FilterOperator.GreaterThanOrEqual;
                case "in":
                    return FilterOperator.In;
                case "!in":
                    return FilterOperator.NotIn;
                case "startswith":
                    return FilterOperator.StartsWith;
                case "!startswith":
                    return FilterOperator.NotStartsWith;
                case "endswith":
                    return FilterOperator.EndsWith;
                case "!endswith":
                    return FilterOperator.NotEndsWith;
                case "contains":
                    return FilterOperator.Contains;
                case "!contains":
                    return FilterOperator.NotContains;
                default:
                    throw new InvalidCastException();
            }            
        }
    }
}
