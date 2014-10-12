using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace VaBank.Common.Data.Repositories
{
    public interface IRepository<TEntity> : IRepository
        where TEntity : class 
    {
        TEntity Find(params object[] keys);
        TModel FindAndProject<TModel>(params object[] keys) where TModel : class;
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IList<TEntity> FindAll();
        IList<TModel> ProjectAll<TModel>() where TModel : class;
    }
}
