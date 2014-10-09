using VaBank.Common.Data.Repositories;

namespace VaBank.Core.Membership
{
    public interface IApplicationTokenRepository : IRepository
    {
        void Create(ApplicationToken token);

        ApplicationToken GetAndDelete(string id);
    }
}
