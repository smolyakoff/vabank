using System.Collections.Generic;

namespace VaBank.Core.Common.History
{
    public interface IHistoricalRepository
    {
        IList<THistoricalEntity> GetAllVersions<THistoricalEntity>(params object[] originalKeys) 
            where THistoricalEntity : class, IHistoricalEntity<THistoricalEntity>;

        THistoricalEntity GetLastVersion<THistoricalEntity>(params object[] originalKeys) 
            where THistoricalEntity : class, IHistoricalEntity<THistoricalEntity>;
    }
}
