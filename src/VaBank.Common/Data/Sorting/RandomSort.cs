using System;
using System.Linq;

namespace VaBank.Common.Data.Sorting
{
    public class RandomSort : ISort
    {
        public Func<IQueryable<T>, IQueryable<T>> ToDelegate<T>()
        {
            return x => x;
        }
    }
}