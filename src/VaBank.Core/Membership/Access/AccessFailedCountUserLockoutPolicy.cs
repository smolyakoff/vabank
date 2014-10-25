using System;
using VaBank.Common.IoC;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Membership.Access
{
    [Injectable(Lifetime = Lifetime.Singleton)]
    public class AccessFailedCountUserLockoutPolicy : IUserLockoutPolicy
    {
        public bool ShouldBeBlocked(User user)
        {
            return !user.LockoutEnabled && user.AccessFailedCount >= 3;
        }

        public bool ShouldBeUnblocked(User user)
        {
            var now = DateTime.UtcNow;
            return user.LockoutEnabled && user.LockoutEndDateUtc.HasValue & user.LockoutEndDateUtc < now;
        }

        public void Block(User user)
        {
            user.LockoutEnabled = true;
            user.LockoutEndDateUtc = DateTime.MaxValue;
        }

        public void Unblock(User user)
        {
            user.LockoutEnabled = false;
            user.LockoutEndDateUtc = null;
            user.AccessFailedCount = 0;
        }
    }
}
