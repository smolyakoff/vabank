using System;
using System.Linq;

namespace VaBank.Common.Data.Repositories
{
    public static class RepositoryExtensions
    {
        public static TEntity QueryOne<TEntity>(this IQueryRepository<TEntity> repository, IQuery query)
            where TEntity : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return repository.Query(query).FirstOrDefault();
        }

        public static TModel ProjectOne<TEntity, TModel>(this IQueryRepository<TEntity> repository, IQuery query)
           where TEntity : class 
           where TModel : class 
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            return repository.Project<TModel>(query).FirstOrDefault();
        }

        public static TEntity QueryIdentity<TKey, TEntity>(this IRepository<TEntity> repository,
            IIdentityQuery<TKey> identityQuery)
            where TEntity : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (identityQuery == null)
            {
                throw new ArgumentNullException("identityQuery");
            }
            return repository.Find(identityQuery.Id);
        }

        public static TModel ProjectIdentity<TKey, TEntity, TModel>(this IRepository<TEntity> repository,
            IIdentityQuery<TKey> identityQuery)
            where TEntity : class
            where TModel : class
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (identityQuery == null)
            {
                throw new ArgumentNullException("identityQuery");
            }
            return repository.FindAndProject<TModel>(identityQuery.Id);
        }
    }
}
