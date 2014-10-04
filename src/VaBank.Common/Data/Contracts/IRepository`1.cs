using System.Collections.Generic;

namespace VaBank.Common.Data.Contracts
{
    public interface IRepository<TEntity>
        where TEntity : class 
    {
        TEntity Find(params object[] keys);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IEnumerable<TEntity> ReadAll();
        IEnumerable<TEntity> Query(IQuery query);
    }
}
