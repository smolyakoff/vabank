using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using DynamicExpression = VaBank.Common.Data.Linq.Dynamic.DynamicExpression;

namespace VaBank.Common.Data.Filtering
{
    public class DynamicLinqFilter : IFilter
    {
        private readonly Dictionary<Type, LambdaExpression> _parsedLambdas = new Dictionary<Type, LambdaExpression>();

        private readonly object[] _parameterValues;

        public DynamicLinqFilter(string linqExpression, params object[] parameterValues)
        {
            LinqExpression = string.IsNullOrEmpty(linqExpression) ? "1 == 1" : linqExpression;
            _parameterValues = parameterValues ?? new object[] {};
        }

        [JsonProperty]
        public string LinqExpression { get; private set; }

        [JsonProperty]
        public IEnumerable<object> ParameterValues
        {
            get { return _parameterValues.ToList(); }
        }

        public Expression<Func<T, bool>> ToExpression<T>()
            where T : class
        {
            if (!_parsedLambdas.ContainsKey(typeof(T)))
            {
                var expression = DynamicExpression.ParseLambda(
                    typeof (T), 
                    typeof (bool), 
                    LinqExpression,
                    _parameterValues);
                _parsedLambdas.Add(typeof(T), expression);
            }
            return (Expression<Func<T, bool>>)_parsedLambdas[typeof(T)];
        }
    }
}
