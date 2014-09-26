using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Common.Expressions
{
    public static class ExpressionHelper
    {
        public static Expression BuildLambdaSelectorBody(Type paramType, string paramName, string propertyName)
        {
            return BuildLambdaSelectorBody(Expression.Parameter(paramType, paramName), propertyName);
        }

        public static Expression BuildLambdaSelectorBody(Expression param, string propertyName)
        {
            return Expression.Property(param, propertyName);
        }

        public static LambdaExpression BuildLambdaSelector(Type paramType, string paramName, string propertyName)
        {
            var param = Expression.Parameter(paramType, paramName);
            var body = BuildLambdaSelectorBody(param, propertyName);
            return Expression.Lambda(body, param);
        }

        public static Expression<Func<TParam, TProperty>> BuildLambdaSelector<TParam, TProperty>(string paramName, string propertyName)
        {
            var param = Expression.Parameter(typeof(TParam), paramName);
            var body = BuildLambdaSelectorBody(param, propertyName);
            return Expression.Lambda<Func<TParam, TProperty>>(body, param);
        }        
    }
}
