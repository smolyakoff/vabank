using System;
using System.Collections.Generic;
using System.Data.Entity;
using VaBank.Core.Data;
using VaBank.Core.Repositories;

namespace VaBank.Data.EntityFramework.Repositories
{
    public class Repository<TEntity>: BaseRepository, IRepository<TEntity>
        where TEntity : class
    {
        private readonly IQueryProcessor _queryProcessor;

        //public Repository() : base(new VaBankContext())
        //{
        //}

        //public Repository(DbContext context): base(context)
        //{
        //    _queryProcessor = new QueryProcessor(context);
        //}

        public Repository(DbContext context, IQueryProcessor queryProcessor) : base(context)
        {
            if (queryProcessor == null)
                throw new ArgumentNullException("queryProcessor", "Query processor can't be null");
            _queryProcessor = queryProcessor;   
        }

        public TEntity Find(params object[] keys)
        {
            return Context.Set<TEntity>().Find(keys);
        }

        public void Create(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            Context.Set<TEntity>().Attach(entity);
        }

        public void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public IEnumerable<TEntity> ReadAll()
        {
            return Context.Set<TEntity>();
        }

        public IEnumerable<TEntity> QueryAll(IQuery<TEntity> query)
        {
            return _queryProcessor.QueryAll(query);
        }
    }
}
