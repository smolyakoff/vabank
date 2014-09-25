using System;
using System.Linq.Expressions;

namespace VaBank.Core.Data
{
    public interface IFilterableQuery<T> : IQuery<T>
        where T : class
    {
        bool InMemoryFiltering { get; }

        Expression<Func<T, bool>> Filtering();
    }
}
