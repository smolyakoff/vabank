﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PagedList;

namespace VaBank.Common.Data.Repositories
{
    public interface IQueryRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        IList<TEntity> Query(IQuery query);

        long Delete(IQuery query);

        long Count(IQuery query);

        long Count();

        IPagedList<TEntity> QueryPage(IQuery query);

        IList<TModel> Project<TModel>(IQuery query)
            where TModel : class;

        IList<T> Select<T>(IQuery query, Expression<Func<TEntity, T>> selector);
            
        IPagedList<TModel> ProjectPage<TModel>(IQuery query)
            where TModel : class;

        IList<TModel> ProjectThenQuery<TModel>(IQuery query)
            where TModel : class;

        IPagedList<TModel> ProjectThenQueryPage<TModel>(IQuery query)
            where TModel : class;
    }
}
