using System;

namespace VaBank.Services.Contracts.Processing.Commands
{
    public interface ICardCommand
    {
        Guid FromCardId { get; set; }

        decimal Amount { get; set; }
    }
}
