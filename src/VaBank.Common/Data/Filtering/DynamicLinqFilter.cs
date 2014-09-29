using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace VaBank.Common.Data.Filtering
{
    public class DynamicLinqFilter : IFilter
    {
        private readonly Dictionary<Type, LambdaExpression> _parsedLambdas = new Dictionary<Type, LambdaExpression>(); 

        public DynamicLinqFilter(string linqExpression, params object[] parameterValues)
        {
            LinqExpression = string.IsNullOrEmpty(linqExpression) ? "1 == 1" : linqExpression;
            ParameterValues = parameterValues ?? new object[] {};
        }

        private DynamicLinqFilter()
        {
        }

        [JsonProperty]
        public string LinqExpression { get; private set; }

        [JsonProperty]
        public object[] ParameterValues { get; private set; }

        public Expression<Func<T, bool>> ToExpression<T>()
            where T : class
        {
            if (!_parsedLambdas.ContainsKey(typeof(T)))
            {
                var expression = Linq.DynamicExpression.ParseLambda(
                    typeof (T), 
                    typeof (bool), 
                    LinqExpression,
                    ParameterValues);
                _parsedLambdas.Add(typeof(T), expression);
            }
            return (Expression<Func<T, bool>>)_parsedLambdas[typeof(T)];
        }
    }
}
