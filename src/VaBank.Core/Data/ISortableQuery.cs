using System;
using System.Linq;

namespace VaBank.Core.Data
{
    public interface ISortableQuery<T> : IQuery<T> 
        where T : class
    {
        bool InMemorySorting { get; }

        Func<IQueryable, IQueryable> Sorting();
    }
}
