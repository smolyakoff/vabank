using System;
using System.Linq.Expressions;

namespace VaBank.Common.Data.Filtering
{
    public class EmptyFilter : IFilter
    {
        public Expression<Func<T, bool>> ToExpression<T>() where T : class
        {
            return x => true;
        }
    }
}
