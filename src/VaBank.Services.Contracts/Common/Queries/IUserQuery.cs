using System;

namespace VaBank.Services.Contracts.Common.Queries
{
    public interface IUserQuery
    {
        Guid UserId { get; set; }
    }
}
