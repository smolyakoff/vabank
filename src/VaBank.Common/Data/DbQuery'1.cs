using System;
using System.Linq.Expressions;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Sorting;

namespace VaBank.Common.Data
{
    public class DbQuery<T> : IFilterableQuery, ISortableQuery
        where T : class
    {
        private bool _inMemeoryFiltering = false;

        private bool _inMemorySotring = false;

        private ISort _sort = new RandomSort();

        private IFilter _filter = new AlwaysTrueFilter();

        internal DbQuery()
        {
        }

        ISort ISortableQuery.Sort
        {
            get { return _sort; }
        }

        bool ISortableQuery.InMemorySorting
        {
            get { return _inMemorySotring; }
        }

        IFilter IFilterableQuery.Filter
        {
            get { return _filter; }
        }

        bool IFilterableQuery.InMemoryFiltering
        {
            get { return _inMemeoryFiltering; }
        }

        public DbQuery<T> WithFilter(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            _filter = new ExpressionFilter<T>(expression);
            return this;
        } 
    }
}
