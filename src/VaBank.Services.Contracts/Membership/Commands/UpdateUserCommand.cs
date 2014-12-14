using System;
using VaBank.Services.Contracts.Common.Commands;

namespace VaBank.Services.Contracts.Membership.Commands
{
    public class UpdateUserCommand : CreateUserCommand, IUserCommand
    {
        public Guid UserId { get; set; }

        public bool ChangePassword { get; set; }

    }
}
