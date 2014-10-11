using System;

namespace VaBank.Services.Contracts.Membership.Models
{
    public interface IUserModel
    {
        Guid UserId { get; set; }
    }
}
