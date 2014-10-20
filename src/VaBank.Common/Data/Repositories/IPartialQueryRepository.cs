using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PagedList;

namespace VaBank.Common.Data.Repositories
{
    public interface IPartialQueryRepository<TEntity> : IQueryRepository<TEntity>
        where TEntity : class 
    {
        IList<TEntity> PartialQuery(Expression<Func<TEntity, bool>> filter, IQuery query);

        IPagedList<TEntity> PartialQueryPage(Expression<Func<TEntity, bool>> filter, IQuery query);

        IList<TModel> PartialProject<TModel>(Expression<Func<TEntity, bool>> filter, IQuery query)
            where TModel : class;

        IPagedList<TModel> PartialProjectPage<TModel>(Expression<Func<TEntity, bool>> filter, IQuery query)
            where TModel : class;

        IList<TModel> PartialProjectThenQuery<TModel>(Expression<Func<TEntity, bool>> filter, IQuery query)
            where TModel : class;

        IPagedList<TModel> PartialProjectThenQueryPage<TModel>(Expression<Func<TEntity, bool>> filter, IQuery query)
            where TModel : class;
    }
}
