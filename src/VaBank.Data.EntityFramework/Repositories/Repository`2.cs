using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Entities;
using VaBank.Core.Repositories;

namespace VaBank.Data.EntityFramework.Repositories
{
    public class Repository<TEntity, TId>: Repository, IRepository<TEntity, TId> where TEntity: Entity<TId>
    {
        public Repository(VaBankContext context): base(context)
        {
        }

        public TEntity Find(TId id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public void Create(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            context.Set<TEntity>().Attach(entity);
        }

        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> ReadAll()
        {
            return context.Set<TEntity>().AsQueryable<TEntity>();
        }

        public IQueryable<TEntity> Filter(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().Where(predicate).AsQueryable<TEntity>();
        }
    }
}
