using System;
using System.Linq;
using System.Linq.Expressions;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Linq;
using VaBank.Common.Data.Sorting;

namespace VaBank.Common.Data
{
    public class DbQuery<T> : IFilterableQuery, ISortableQuery
        where T : class
    {
        private bool _inMemoryFiltering = false;

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
            get { return _inMemoryFiltering; }
        }

        public DbQuery<T> SortBy(ISort sort)
        {
            if (sort == null)
            {
                throw new ArgumentNullException("sort");
            }
            _sort = sort;
            return this;
        }

        public virtual DbQuery<T> FromClientQuery(IClientQuery clientQuery)
        {
            if (clientQuery == null)
            {
                throw new ArgumentNullException("clientQuery");
            }
            var filterable = clientQuery as IClientFilterable;
            var sortable = clientQuery as IClientSortable;
            if (filterable != null)
            {
                _filter = filterable.ClientFilter ?? new AlwaysTrueFilter();
            }
            if (sortable != null)
            {
                _sort = sortable.ClientSort ?? new RandomSort();
            }
            return this;
        }

        public DbQuery<T> SortBy(Func<IQueryable<T>, IQueryable<T>> sort)
        {
            if (sort == null)
            {
                throw new ArgumentNullException("sort");
            }
            _sort = new DelegateSort<T>(sort);
            return this;
        }

        public DbQuery<T> FilterBy(IFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            _filter = filter;
            return this;
        }

        public DbQuery<T> FilterBy(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            _filter = new ExpressionFilter<T>(expression);
            return this;
        }

        public DbQuery<T> AndFilterBy(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (_filter == null)
            {
                _filter = new ExpressionFilter<T>(expression);
            }
            _filter = _filter.And(new ExpressionFilter<T>(expression));
            return this;
        }

        public DbQuery<T> OrFilterBy(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (_filter == null)
            {
                _filter = new ExpressionFilter<T>(expression);
            }
            _filter = _filter.Or(new ExpressionFilter<T>(expression));
            return this;
        }
    }
}
