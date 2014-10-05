using System.Collections.Generic;

namespace VaBank.Common.Data.Repositories
{
    public interface IRepository<TEntity> : IRepository
        where TEntity : class 
    {
        TEntity Find(params object[] keys);
        TModel Project<TModel>(params object[] keys) where TModel : class;
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(params object[] keys);
        IList<TEntity> FindAll();
        IList<TModel> ProjectAll<TModel>() where TModel : class;
    }
}
