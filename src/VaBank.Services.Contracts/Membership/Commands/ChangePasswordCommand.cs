using System;
using VaBank.Services.Contracts.Common.Commands;

namespace VaBank.Services.Contracts.Membership.Commands
{
    public class ChangePasswordCommand : IUserCommand
    {
        public Guid UserId { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordConfirmation { get; set; }
    }
}
