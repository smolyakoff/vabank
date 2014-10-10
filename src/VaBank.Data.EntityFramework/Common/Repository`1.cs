using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EntityFramework.Extensions;
using PagedList;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;

namespace VaBank.Data.EntityFramework.Common
{
    public class Repository<TEntity> : IQueryRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context", "DbContext should not be empty");
            }
            Context = context;
        }

        public virtual TEntity Find(params object[] keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException("keys");
            }
            return EnsureRepositoryException(() => Context.Set<TEntity>().Find(keys));
        }

        public virtual TModel FindAndProject<TModel>(params object[] keys) where TModel : class
        {
            if (keys == null)
            {
                throw new ArgumentNullException("keys");
            }
            return EnsureRepositoryException(() => Mapper.Map<TEntity, TModel>(Context.Set<TEntity>().Find(keys)));
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
            return EnsureRepositoryException(() => Context.Set<TEntity>().ToList());
        }

        public virtual IList<TModel> ProjectAll<TModel>() where TModel : class
        {
            return EnsureRepositoryException(() => Context.Set<TEntity>().AsQueryable().Project().To<TModel>().ToList());
        }

        public virtual IList<TEntity> Query(IQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.Set<TEntity>().AsQueryable().Query(query).ToList());
        }

        public virtual long Delete(IQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.Set<TEntity>().AsQueryable().Query(query).Delete());
        }

        public virtual long Count(IQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.Set<TEntity>().AsQueryable().Query(query).Count());
        }

        public virtual long Count()
        {
            return EnsureRepositoryException(() => Context.Set<TEntity>().Count());
        }

        public virtual IPagedList<TEntity> QueryPage(IQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.Set<TEntity>().AsQueryable().QueryPage(query));
        }

        public virtual IList<TModel> Project<TModel>(IQuery query) where TModel : class
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.Set<TEntity>().AsQueryable().Query(query).Project().To<TModel>().ToList());
        }

        public virtual IPagedList<TModel> ProjectPage<TModel>(IQuery query) where TModel : class
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => MapPage<TModel>(Context.Set<TEntity>().AsQueryable().QueryPage(query)));
        }

        public virtual IList<TModel> ProjectThenQuery<TModel>(IQuery query) where TModel : class
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.Set<TEntity>().AsQueryable().Project().To<TModel>().Query(query).ToList());
        }

        public virtual IPagedList<TModel> ProjectThenQueryPage<TModel>(IQuery query) where TModel : class
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return EnsureRepositoryException(() => Context.Set<TEntity>().AsQueryable().Project().To<TModel>().QueryPage(query));
        }

        private IPagedList<TModel> MapPage<TModel>(IPagedList<TEntity> page) where TModel : class
        {
            var models = page.Select(Mapper.Map<TEntity, TModel>);
            return new StaticPagedList<TModel>(models, page.PageNumber, page.PageSize, page.TotalItemCount);
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