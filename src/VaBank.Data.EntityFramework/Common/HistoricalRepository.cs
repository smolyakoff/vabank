using System;
using System.Collections.Generic;
using System.Data.Entity;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.Common;
using System.Linq;
using System.Linq.Expressions;

namespace VaBank.Data.EntityFramework.Common
{
    public class HistoricalRepository : IRepository, IHistoricalRepository
    {
        protected readonly DbContext Context;

        public HistoricalRepository(DbContext context)
        {
            Argument.NotNull(context, "context");
            Context = context;
        }
        
        public IList<THistoricalEntity> GetAllBySurrogateKey<THistoricalEntity>(object surrogateKey) where THistoricalEntity : class, IHistoricalEntity<THistoricalEntity>
        {
            Argument.NotNull(surrogateKey, "surrogateKey");

            return EnsureRepositoryException(() =>
            {
                return Context.Set<THistoricalEntity>().Where(GetSurrogateKeyFilterExpression<THistoricalEntity>(surrogateKey)).ToList();
            });
        }

        public THistoricalEntity GetLast<THistoricalEntity>(object surrogateKey) where THistoricalEntity : class, IHistoricalEntity<THistoricalEntity>
        {
            Argument.NotNull(surrogateKey, "surrogateKey");

            return EnsureRepositoryException(() =>
            {
                return Context.Set<THistoricalEntity>().Where(GetSurrogateKeyFilterExpression<THistoricalEntity>(surrogateKey)).OrderBy(x => x.HistoryTimestampUtc).LastOrDefault();
            });
        }

        public IList<THistoricalEntity> GetAll<THistoricalEntity>() where THistoricalEntity : class, IHistoricalEntity<THistoricalEntity>
        {
            return EnsureRepositoryException(() =>
            {
                return Context.Set<THistoricalEntity>().ToList();
            });
        }

        private Expression<Func<THistoricalEntity, bool>> GetSurrogateKeyFilterExpression<THistoricalEntity>(object surrogateKey) 
            where THistoricalEntity : class, IHistoricalEntity<THistoricalEntity>
        {
            var obj = (THistoricalEntity)Activator.CreateInstance(typeof(THistoricalEntity), true);
            var parameter = Expression.Parameter(surrogateKey.GetType(), "x");
            Expression body = Expression.Property(parameter, obj.SurrogateKeyPropertyName);
            body = Expression.Equal(body, Expression.Constant(surrogateKey));
            return Expression.Lambda<Func<THistoricalEntity, bool>>(body, parameter);
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
