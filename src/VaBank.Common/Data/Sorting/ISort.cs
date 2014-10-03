using System;
using System.Linq;

namespace VaBank.Common.Data.Sorting
{
    public interface ISort
    {
        Func<IQueryable<T>, IQueryable<T>> ToDelegate<T>();
    }
}
