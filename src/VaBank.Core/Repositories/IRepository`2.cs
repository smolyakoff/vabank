using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Entities;

namespace VaBank.Core.Repositories
{
    public interface IRepository<TEntity, TKey> : IRepository where TEntity: Entity<TKey>
    {
        void Create(TEntity entity);
        TEntity Read(TKey id);
        void Update(TEntity entity);
        void Delete(TKey id);
        IQueryable<TEntity> ReadAll();
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);
    }
}
