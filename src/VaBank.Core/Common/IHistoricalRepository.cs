using System.Collections.Generic;

namespace VaBank.Core.Common
{
    public interface IHistoricalRepository
    {
        IList<THistoricalEntity> GetAllBySurrogateKey<THistoricalEntity>(object surrogateKey) where THistoricalEntity : class, IHistoricalEntity<THistoricalEntity>;

        THistoricalEntity GetLast<THistoricalEntity>(object surrogateKey) where THistoricalEntity : class, IHistoricalEntity<THistoricalEntity>;

        IList<THistoricalEntity> GetAll<THistoricalEntity>() where THistoricalEntity : class, IHistoricalEntity<THistoricalEntity>;
    }
}
