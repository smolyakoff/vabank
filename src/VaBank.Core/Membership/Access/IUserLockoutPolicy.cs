using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Membership.Access
{
    public interface IUserLockoutPolicy
    {
        bool ShouldBeBlocked(User user);

        bool ShouldBeUnblocked(User user);

        void Block(User user);

        void Unblock(User user);
    }
}
