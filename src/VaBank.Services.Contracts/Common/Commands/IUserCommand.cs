using System;

namespace VaBank.Services.Contracts.Common.Commands
{
    public interface IUserCommand
    {
        Guid UserId { get; set; }
    }
}
