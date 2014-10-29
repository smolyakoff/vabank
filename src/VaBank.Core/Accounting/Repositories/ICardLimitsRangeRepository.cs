using System.Collections.Generic;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Accounting.Repositories
{
    public interface ICardLimitsRangeRepository
    {
        IList<CardLimitsRange> GetAll();

        CardLimitsRange GetWithISOName(string isoName);
    }
}
