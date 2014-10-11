using System;

namespace VaBank.Services.Contracts.Membership.Commands
{
    public interface IUserCommand
    {
        Guid UserId { get; set; }
    }
}
