using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Common.Data.Contracts;

namespace VaBank.Data.EntityFramework.Repositories
{
    public class Repository<TEntity>: BaseRepository, IRepository<TEntity>
        where TEntity : class
    {
        public Repository(DbContext context) : base(context)
        {
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

        public IEnumerable<TEntity> Query(IQuery query)
        {
            return Context.Set<TEntity>().AsQueryable().Query(query);
        }
    }
}
