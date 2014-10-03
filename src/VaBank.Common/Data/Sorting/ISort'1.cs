using System;
using System.Linq;

namespace VaBank.Common.Data.Sorting
{
    public interface ISort<T> : ISort
        where T : class
    {
        Func<IQueryable<T>, IQueryable<T>> ToDelegate();
    }
}