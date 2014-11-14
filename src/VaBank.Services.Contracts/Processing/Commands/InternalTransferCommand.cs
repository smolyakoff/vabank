using System;

namespace VaBank.Services.Contracts.Processing.Commands
{
    public class InternalTransferCommand
    {
        public Guid FromCardId { get; set; }

        public Guid ToCardId { get; set; }

        public decimal TransferAmount { get; set; }
    }
}
