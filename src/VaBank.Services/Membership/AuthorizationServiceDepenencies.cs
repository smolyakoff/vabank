using VaBank.Common.Data.Repositories;
using VaBank.Core.Membership;
using VaBank.Core.Membership.Access;
using VaBank.Core.Membership.Entities;
using VaBank.Core.Membership.Repositories;
using VaBank.Services.Common;

namespace VaBank.Services.Membership
{
    public class AuthorizationServiceDepenencies : IDependencyCollection
    {
        public IApplicationTokenRepository ApplicationTokens { get; set; }
        public IQueryRepository<ApplicationClient> ApplicationClients { get; set; }
        public IQueryRepository<User> Users { get; set; }
        public IAccessPolicy UserAccessPolicy { get; set; }
    }
}
