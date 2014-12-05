using System;
using VaBank.Services.Contracts.Common.Commands;

namespace VaBank.Services.Contracts.Processing.Commands
{
    public interface ICardWithdrawalCommand : ISecurityCodeCommand
    {
        Guid FromCardId { get; }

        decimal Amount { get; }
    }
}
