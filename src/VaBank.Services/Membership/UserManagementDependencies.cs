using VaBank.Common.Data.Repositories;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Membership.Access;
using VaBank.Core.Membership.Entities;
using VaBank.Core.Membership.Factories;
using VaBank.Services.Common;

namespace VaBank.Services.Membership
{
    public class UserManagementDependencies : IDependencyCollection
    {
        public UserPaymentProfileFactory UserPaymentProfileFactory { get; set; }

        public IPartialQueryRepository<User> Users { get; set; } 

        public IQueryRepository<UserProfile> UserProfiles { get; set; }

        public IQueryRepository<UserClaim> UserClaims { get; set; } 

        public IQueryRepository<ApplicationToken> Tokens { get; set; } 

        public IRepository<UserPaymentProfile> PaymentProfiles { get; set; }

        public IUserLockoutPolicy UserLockoutPolicy { get; set; }
    }
}
