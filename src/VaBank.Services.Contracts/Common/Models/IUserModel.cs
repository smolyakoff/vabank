using System;

namespace VaBank.Services.Contracts.Common.Models
{
    public interface IUserModel
    {
        Guid UserId { get; set; }
    }
}
