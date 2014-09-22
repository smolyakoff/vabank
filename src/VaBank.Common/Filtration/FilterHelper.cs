using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Common.Filtration
{
    public static class FilterHelper
    {
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
                    return FilterOperator.Equality;
                case "!=":
                    return FilterOperator.Inequality;
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
                case "notin":
                    return FilterOperator.NotIn;
                case "startswith":
                    return FilterOperator.StartsWith;
                case "notstartswith":
                    return FilterOperator.NotStartsWith;
                case "endswith":
                    return FilterOperator.EndsWith;
                case "notendswith":
                    return FilterOperator.NotEndsWith;
                case "contains":
                    return FilterOperator.Contains;
                case "notcontains":
                    return FilterOperator.NotContains;
                default:
                    throw new InvalidCastException();
            }            
        }
    }
}
