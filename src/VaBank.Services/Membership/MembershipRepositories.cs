using VaBank.Common.Data.Repositories;
using VaBank.Core.Membership;
using VaBank.Services.Common;

namespace VaBank.Services.Membership
{
    public class MembershipRepositories : IRepositoryCollection
    {
        public IQueryRepository<ApplicationToken> ApplicationTokens { get; set; }
        public IQueryRepository<ApplicationClient> ApplicationClients { get; set; }
        public IQueryRepository<User> Users { get; set; }
    }
}
