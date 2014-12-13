using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EntityFramework.Extensions;
using PagedList;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;

namespace VaBank.Data.EntityFramework.Common
{
    public class Repository<TEntity> : IPartialQueryRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext Context;

        protected readonly string[] Includes;

        public Repository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "DbContext should not be empty");
            }
            Context = context;
            var includeAttribute = typeof (TEntity).GetCustomAttribute<IncludeAttribute>();
            Includes = includeAttribute == null 
                ? new string[0] 
                : includeAttribute.Included;
        }

        public virtual TEntity Find(params object[] keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException("keys");
            }
            return EnsureRepositoryException(() => Context.Find<TEntity>(Includes, keys));
        }

        public virtual TModel FindAndProject<TModel>(params object[] keys) where TModel : class
        {
            if (keys == null)
            {
                throw new ArgumentNullException("keys");
            }
            return EnsureRepositoryException(() => Mapper.Map<TEntity, TModel>(Context.Find<TEntity>(Includes, keys)));
        }

        public virtual void Create(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            EnsureRepositoryException(() => Context.Set<TEntity>().Add(entity));
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            EnsureRepositoryException(() => Context.Entry(entity).State = EntityState.Modified);
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            EnsureRepositoryException(() => Context.Set<TEntity>().Remove(entity));
        }


        public virtual IList<TEntity> FindAll()
        {
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).ToList());
        }

        public virtual IList<TModel> ProjectAll<TModel>() where TModel : class
        {
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).AsQueryable().Project().To<TModel>().ToList());
        }

        public virtual IList<T> SelectAll<T>(Expression<Func<TEntity, T>> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).Select(selector).ToList());
        } 

        public virtual IList<TEntity> Query(IQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).AsQueryable().Query(query).ToList());
        }

        public virtual long Delete(IQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).AsQueryable().Query(query).Delete());
        }

        public virtual long Count(IQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).AsQueryable().Query(query).Count());
        }

        public virtual long Count()
        {
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).Count());
        }

        public virtual IPagedList<TEntity> QueryPage(IQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).AsQueryable().QueryPage(query));
        }

        public virtual IList<TModel> Project<TModel>(IQuery query) where TModel : class
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).AsQueryable().Query(query).Project().To<TModel>().ToList());
        }

        public virtual IList<T> Select<T>(IQuery query, Expression<Func<TEntity, T>> selector)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).AsQueryable().Query(query).Select(selector).ToList());
        } 

        public virtual IPagedList<TModel> ProjectPage<TModel>(IQuery query) where TModel : class
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => MapPage<TModel>(Context.SetIncluding<TEntity>(Includes).AsQueryable().QueryPage(query)));
        }

        public virtual IList<TModel> ProjectThenQuery<TModel>(IQuery query) where TModel : class
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).AsQueryable().Project().To<TModel>().Query(query).ToList());
        }

        public virtual IPagedList<TModel> ProjectThenQueryPage<TModel>(IQuery query) where TModel : class
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).AsQueryable().Project().To<TModel>().QueryPage(query));
        }

        private IPagedList<TModel> MapPage<TModel>(IPagedList<TEntity> page) where TModel : class
        {
            var models = page.Select(Mapper.Map<TEntity, TModel>);
            return new StaticPagedList<TModel>(models, page.PageNumber, page.PageSize, page.TotalItemCount);
        }

        public IList<TEntity> PartialQuery(Expression<Func<TEntity, bool>> filter, IQuery query)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).Where(filter).Query(query).ToList());
        }

        public IPagedList<TEntity> PartialQueryPage(Expression<Func<TEntity, bool>> filter, IQuery query)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).Where(filter).QueryPage(query));
        }

        public IList<TModel> PartialProject<TModel>(Expression<Func<TEntity, bool>> filter, IQuery query) where TModel : class
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).Where(filter).Query(query).Project().To<TModel>().ToList());
        }

        public IPagedList<TModel> PartialProjectPage<TModel>(Expression<Func<TEntity, bool>> filter, IQuery query) where TModel : class
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => MapPage<TModel>(Context.SetIncluding<TEntity>(Includes).Where(filter).QueryPage(query)));
        }

        public IList<TModel> PartialProjectThenQuery<TModel>(Expression<Func<TEntity, bool>> filter, IQuery query) where TModel : class
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).Where(filter).Project().To<TModel>().Query(query).ToList());
        }

        public IPagedList<TModel> PartialProjectThenQueryPage<TModel>(Expression<Func<TEntity, bool>> filter, IQuery query) where TModel : class
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.SetIncluding<TEntity>(Includes).Where(filter).Project().To<TModel>().QueryPage(query));
        }

        protected T EnsureRepositoryException<T>(Func<T> call)
        {
            try
            {
                return call();
            }
            catch (RepositoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(ex.Message, ex);
            }
        }
    }
}