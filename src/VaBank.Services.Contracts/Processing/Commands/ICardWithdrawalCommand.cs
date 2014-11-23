using System;

namespace VaBank.Services.Contracts.Processing.Commands
{
    public interface ICardWithdrawalCommand
    {
        Guid FromCardId { get; set; }

        decimal Amount { get; set; }
    }
}
