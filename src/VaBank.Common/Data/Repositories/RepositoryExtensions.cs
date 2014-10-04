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
    }
}
