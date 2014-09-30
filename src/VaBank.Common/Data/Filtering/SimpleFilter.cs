using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VaBank.Common.Data.Filtering
{
    public class SimpleFilter : IFilter
    {
        private SimpleFilter()
        {
        }

        [JsonProperty(Required = Required.Always)]
        public string PropertyName { get; private set; }

        [JsonProperty(Required = Required.Always)]
        public FilterPropertyType PropertyType { get; private set; }

        [JsonProperty(Required = Required.Always)]
        public FilterOperator Operator { get; private set; }

        [JsonProperty(Required = Required.Always)]
        public object Value { get; private set; }

        public Expression<Func<T ,bool>> ToExpression<T>()
            where T : class 
        {
            var dynamicLinqFilter = ToDynamicLinqFilter();
            return dynamicLinqFilter.ToExpression<T>();
        }

        public DynamicLinqFilter ToDynamicLinqFilter()
        {
            var expression = ToDynamicLinqExpression();
            return new DynamicLinqFilter(expression.Expression, expression.Parameters);
        }

        private DynamicLinqExpression ToDynamicLinqExpression()
        {
            switch (Operator)
            {
                case FilterOperator.Equal:
                    return new DynamicLinqExpression(string.Format("{0} == @0", PropertyName), Value);
                case FilterOperator.NotEqual:
                    return new DynamicLinqExpression(string.Format("{0} != @0", PropertyName), Value);
                case FilterOperator.GreaterThan:
                    return new DynamicLinqExpression(string.Format("{0} > @0", PropertyName), Value);
                case FilterOperator.LessThan:
                    return new DynamicLinqExpression(string.Format("{0} < @0", PropertyName), Value);
                case FilterOperator.GreaterThanOrEqual:
                    return new DynamicLinqExpression(string.Format("{0} >= @0", PropertyName), Value);
                case FilterOperator.LessThanOrEqual:
                    return new DynamicLinqExpression(string.Format("{0} <= @0", PropertyName), Value);
                case FilterOperator.StartsWith:
                    return new DynamicLinqExpression(string.Format("{0}.StartsWith(@0)", PropertyName), Value);
                case FilterOperator.NotStartsWith:
                    return new DynamicLinqExpression(string.Format("!{0}.StartsWith(@0)", PropertyName), Value);
                case FilterOperator.EndsWith:
                    return new DynamicLinqExpression(string.Format("{0}.EndsWith(@0)", PropertyName), Value);
                case FilterOperator.NotEndsWith:
                    return new DynamicLinqExpression(string.Format("!{0}.EndsWith(@0)", PropertyName), Value);
                case FilterOperator.Contains:
                    return new DynamicLinqExpression(string.Format("{0}.Contains(@0)", PropertyName), Value);
                case FilterOperator.NotContains:
                    return new DynamicLinqExpression(string.Format("!{0}.Contains(@0)", PropertyName), Value);
                case FilterOperator.In:
                    return In();
                case FilterOperator.NotIn:
                    return NotIn();
                default:
                    throw new NotSupportedException(string.Format("Operator [{0}] is not supported.", Operator));
            }
        }

        private DynamicLinqExpression In()
        {
            var value = Value;
            var jArray = value as JArray;
            if (jArray != null)
            {
                value = ToConcreteList(jArray);
            }
            return new DynamicLinqExpression(string.Format("@0.Contains({0})", PropertyName), value);
        }

        private DynamicLinqExpression NotIn()
        {
            var value = Value;
            var jArray = value as JArray;
            if (jArray != null)
            {
                value = ToConcreteList(jArray);
            }
            return new DynamicLinqExpression(string.Format("!@0.Contains({0})", PropertyName), value);
        }

        private object ToConcreteList(JArray array)
        {
            var elementType = PropertyType.ToType();
            var listType = typeof (List<>).MakeGenericType(elementType);
            return array.ToObject(listType);
        }

        private class DynamicLinqExpression
        {
            public DynamicLinqExpression(string expression, params object[] parameters)
            {
                Expression = expression;
                Parameters = parameters;
            }

            public string Expression { get; private set; }

            public object[] Parameters { get; private set; }
        }
    }
}
