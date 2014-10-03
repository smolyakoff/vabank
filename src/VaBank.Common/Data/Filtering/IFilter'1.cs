using System;
using System.Linq.Expressions;

namespace VaBank.Common.Data.Filtering
{
    public interface IFilter<T> : IFilter where T : class
    {
        Expression<Func<T, bool>> ToExpression();
    }
}