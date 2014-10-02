using System;
using System.Linq;

namespace VaBank.Common.Data.Sorting
{
    public interface ISort
    {
        Func<IQueryable<T>, IOrderedQueryable<T>> ToDelegate<T>();
    }
}
