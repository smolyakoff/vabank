using VaBank.Common.Data.Repositories;
using VaBank.Core.Membership.Entities;
using VaBank.Services.Common;

namespace VaBank.Services.Membership
{
    public class UserManagementRepositories : IDependencyCollection
    {
        public IPartialQueryRepository<User> Users { get; set; } 

        public IQueryRepository<UserProfile> UserProfiles { get; set; }

        public IQueryRepository<UserClaim> UserClaims { get; set; } 

        public IQueryRepository<ApplicationToken> Tokens { get; set; } 
    }
}
