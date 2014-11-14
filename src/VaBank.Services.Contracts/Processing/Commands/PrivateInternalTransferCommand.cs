using System;

namespace VaBank.Services.Contracts.Processing.Commands
{
    public class PrivateInternalTransferCommand : InternalTransferCommand
    {
        public DateTime ToCardExpirationDateUtc { get; set; }
    }
}
