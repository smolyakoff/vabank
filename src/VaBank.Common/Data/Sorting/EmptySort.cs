using System;
using System.Linq;

namespace VaBank.Common.Data.Sorting
{
    public class EmptySort : ISort
    {
        public Func<IQueryable<T>, IQueryable<T>> ToDelegate<T>()
        {
            return x => x;
        }
    }
}
