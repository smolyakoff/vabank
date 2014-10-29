using VaBank.Common.Data.Repositories;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Membership.Repositories
{
    public interface IApplicationTokenRepository : IRepository
    {
        void Create(ApplicationToken token);

        ApplicationToken GetAndDelete(string id);
    }
}
