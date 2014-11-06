using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VaBank.Core.Common.Repositories
{
    public interface IHistoryRepository
    {
        THistoricalEntity GetLast<THistoricalEntity>(Expression<Func<THistoricalEntity, bool>> predicate) where THistoricalEntity : HistoricalEntity;
        IList<THistoricalEntity> GetAll<THistoricalEntity>(Expression<Func<THistoricalEntity, bool>> predicate) where THistoricalEntity : HistoricalEntity;
    }
}
