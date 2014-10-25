using System;
using VaBank.Common.IoC;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Membership.Access
{
    [Injectable(Lifetime = Lifetime.Singleton)]
    public class LockoutAccessPolicy : IAccessPolicy
    {
        private readonly IUserLockoutPolicy _lockoutPolicy;

        public LockoutAccessPolicy(IUserLockoutPolicy lockoutPolicy)
        {
            if (lockoutPolicy == null)
            {
                throw new ArgumentNullException("lockoutPolicy");
            }
            _lockoutPolicy = lockoutPolicy;
        }

        public AccessStatus VerifyAccess(User user, string password, bool verifyPassword = true)
        {
            if (user.Deleted)
            {
                user.AccessFailedCount++;
                return AccessStatus.Deleted;
            }
            if (_lockoutPolicy.ShouldBeUnblocked(user))
            {
                _lockoutPolicy.Unblock(user);
            }
            if (user.LockoutEnabled)
            {
                user.AccessFailedCount++;
                return AccessStatus.Blocked;
            }
            var passwordValid = !verifyPassword || user.ValidatePassword(password);
            if (!passwordValid)
            {
                user.AccessFailedCount++;
            }
            if (_lockoutPolicy.ShouldBeBlocked(user))
            {
                _lockoutPolicy.Block(user);
                return AccessStatus.Blocked;
            }
            return !passwordValid 
                ? AccessStatus.BadCredentials 
                : AccessStatus.Allowed;
        }
    }
}
