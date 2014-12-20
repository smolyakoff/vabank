using VaBank.Common.Data.Repositories;

namespace VaBank.Core.Common
{
    public interface IDbInformationRepository : IRepository
    {
        long GetDbVersion();
    }
}
