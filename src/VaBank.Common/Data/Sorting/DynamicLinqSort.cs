using System;
using System.Linq;
using VaBank.Common.Data.Linq.Dynamic;

namespace VaBank.Common.Data.Sorting
{
    public class DynamicLinqSort : ISort
    {
        private readonly string _sortExpression;

        public DynamicLinqSort(string sortExpression)
        {
            _sortExpression = sortExpression;
        }

        public Func<IQueryable<T>, IQueryable<T>> ToDelegate<T>()
        {
            if (string.IsNullOrEmpty(_sortExpression))
            {
                return x => x;
            }
            return x => x.OrderBy(_sortExpression);
        }
    }
}
