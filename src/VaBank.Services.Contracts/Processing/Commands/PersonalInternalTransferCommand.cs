using System;

namespace VaBank.Services.Contracts.Processing.Commands
{
    public class PersonalInternalTransferCommand : InternalTransferCommand
    {
        public DateTime ToCardExpirationDateUtc { get; set; }
    }
}
