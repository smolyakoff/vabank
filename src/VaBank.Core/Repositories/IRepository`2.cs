using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Entities;

namespace VaBank.Core.Repositories
{
    public interface IRepository<TEntity, in TId> : IRepository where TEntity: Entity<TId>
    {
        TEntity Find(TId id);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> ReadAll();
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);
    }
}
