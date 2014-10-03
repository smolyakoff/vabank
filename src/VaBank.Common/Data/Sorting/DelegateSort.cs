using System;
using System.Linq;

namespace VaBank.Common.Data.Sorting
{
    public class DelegateSort<T> : ISort<T>
        where T : class
    {
        private readonly Func<IQueryable<T>, IQueryable<T>> _sorter;

        public DelegateSort(Func<IQueryable<T>, IQueryable<T>> sorter)
        {
            if (sorter == null)
            {
                throw new ArgumentNullException("sorter");
            }
            _sorter = sorter;
        }

        public Func<IQueryable<T>, IQueryable<T>> ToDelegate()
        {
            return _sorter;
        }

        public Func<IQueryable<T1>, IQueryable<T1>> ToDelegate<T1>()
        {
            if (typeof (T1) != typeof (T))
            {
                throw new NotSupportedException("Expression enclosed type mismatch.");
            }
            return _sorter as Func<IQueryable<T1>, IQueryable<T1>>;
        }

        public static implicit operator Func<IQueryable<T>, IQueryable<T>>(DelegateSort<T> sort)
        {
            if (sort == null)
            {
                throw new ArgumentNullException("sort");
            }
            return sort._sorter;
        }

        public static implicit operator DelegateSort<T>(Func<IQueryable<T>, IQueryable<T>> sorter)
        {
            if (sorter == null)
            {
                throw new ArgumentNullException("sorter");
            }
            return new DelegateSort<T>(sorter);
        }
    }
}