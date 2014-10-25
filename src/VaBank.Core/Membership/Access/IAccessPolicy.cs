using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Membership.Access
{
    public interface IAccessPolicy
    {
        AccessStatus VerifyAccess(User user, string password, bool verifyPassword = true);
    }
}
