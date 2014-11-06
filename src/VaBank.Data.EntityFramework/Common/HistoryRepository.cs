using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Common;
using VaBank.Core.Common.Repositories;

namespace VaBank.Data.EntityFramework.Common
{
    public class HistoryRepository : IRepository, IHistoryRepository
    {
        protected readonly DbContext Context;

        public HistoryRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            Context = context;
        }

        public IList<THistoricalEntity> GetAll<THistoricalEntity>(Expression<Func<THistoricalEntity, bool>> predicate)
            where THistoricalEntity : HistoricalEntity
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            return Context.Set<THistoricalEntity>().Where(predicate).ToList();
        }

        public THistoricalEntity GetLast<THistoricalEntity>(Expression<Func<THistoricalEntity, bool>> predicate) where THistoricalEntity : HistoricalEntity
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            return Context.Set<THistoricalEntity>().Where(predicate).OrderBy(x => x.HistoryTimestampUtc).LastOrDefault();
        }
    }
}
