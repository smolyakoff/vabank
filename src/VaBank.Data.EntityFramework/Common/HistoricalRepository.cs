using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.Common.History;

namespace VaBank.Data.EntityFramework.Common
{
    public class HistoricalRepository : IRepository, IHistoricalRepository
    {
        private static readonly Dictionary<Type, object> Specs = new Dictionary<Type, object>(); 

        protected readonly DbContext Context;

        public HistoricalRepository(DbContext context)
        {
            Argument.NotNull(context, "context");
            Context = context;
        }

        public IList<THistoricalEntity> GetAllVersions<THistoricalEntity>(params object[] originalKeys) where THistoricalEntity : class, IHistoricalEntity<THistoricalEntity>
        {
            return EnsureRepositoryException(() =>
            {
                var spec = LoadSpec<THistoricalEntity>();
                var versions = Context.Set<THistoricalEntity>()
                    .Where(spec.OriginalKey(originalKeys).Expression)
                    .OrderByDescending(x => x.HistoryTimestampUtc)
                    .ToList();
                return versions;
            });
        }

        public THistoricalEntity GetLastVersion<THistoricalEntity>(params object[] originalKeys) where THistoricalEntity : class, IHistoricalEntity<THistoricalEntity>
        {
            return EnsureRepositoryException(() =>
            {
                var spec = LoadSpec<THistoricalEntity>();
                var version = Context.Set<THistoricalEntity>()
                    .Where(spec.OriginalKey(originalKeys).Expression)
                    .Where(x => x.HistoryAction != 'D')
                    .OrderByDescending(x => x.HistoryTimestampUtc)
                    .FirstOrDefault();
                return version;
            });
        }

        private static IHistoricalEntitySpecification<T> LoadSpec<T>()
        {
            if (Specs.ContainsKey(typeof (T)))
            {
                return (IHistoricalEntitySpecification<T>) Specs[typeof(T)];
            }
            var attribute = typeof (T).GetCustomAttribute(typeof (HistoricalAttribute)) as HistoricalAttribute;
            if (attribute == null)
            {
                var message = string.Format("No historical spec found for [{0}].", typeof (T).Name);
                throw new InvalidOperationException(message);
            }
            try
            {
                var spec = attribute.Spec<T>();
                Specs[typeof (T)] = spec;
                return spec;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Historical spec type mismatch.", ex);
            }
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
