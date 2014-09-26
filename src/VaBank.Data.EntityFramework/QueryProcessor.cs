using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper.QueryableExtensions;
using VaBank.Core.Data;

namespace VaBank.Data.EntityFramework
{
    //experimental feature
    public class QueryProcessor : IQueryProcessor
    {
        private readonly DbContext _context;

        public QueryProcessor(DbContext context)
        {
            _context = context;
        }

        public IList<TEntity> QueryAll<TEntity>(IQuery<TEntity> query) 
            where TEntity : class
        {
            var queryable = _context.Set<TEntity>().AsQueryable();
            var filterableQuery = query as IFilterableQuery<TEntity>;
            if (filterableQuery != null)
            {
                queryable = filterableQuery.InMemoryFiltering
                    ? queryable.AsEnumerable().Where(filterableQuery.Filtering().Compile()).AsQueryable()
                    : queryable.Where(filterableQuery.Filtering());
            }
            //TODO: sorting and paging...
            return queryable.ToList();
        }

        public IList<TModel> QueryAll<TEntity, TModel>(IQuery<TModel> query) 
            where TModel : class 
            where TEntity : class
        {
            //doing automapper stuff
            //no tracking because of mapping
            var queryable = _context.Set<TEntity>().AsQueryable().AsNoTracking().Project().To<TModel>();
            var filterableQuery = query as IFilterableQuery<TModel>;
            if (filterableQuery != null)
            {
                queryable = filterableQuery.InMemoryFiltering
                    ? queryable.AsEnumerable().Where(filterableQuery.Filtering().Compile()).AsQueryable()
                    : queryable.Where(filterableQuery.Filtering());
            }
            //TODO: sorting and paging...
            return queryable.ToList();
        }
    }
}
