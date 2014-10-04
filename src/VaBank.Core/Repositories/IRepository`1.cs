using System.Collections.Generic;
using VaBank.Core.Data;

namespace VaBank.Core.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class 
    {
        TEntity Find(params object[] keys);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IEnumerable<TEntity> ReadAll();
        IEnumerable<TEntity> QueryAll(IQuery<TEntity> query);
    }
}
