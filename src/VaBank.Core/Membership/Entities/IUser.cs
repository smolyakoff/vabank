using System;

namespace VaBank.Core.Membership.Entities
{
    public interface IUser
    {
        Guid Id { get; }
        string PasswordHash { get; }
        string PasswordSalt { get; }
        bool LockoutEnabled { get; }
        DateTime? LockoutEndDateUtc { get; }
        string UserName { get; }
        int AccessFailedCount { get; }
        bool Deleted { get; }
    }
}
