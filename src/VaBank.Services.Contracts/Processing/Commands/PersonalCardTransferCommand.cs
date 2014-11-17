using System;
using VaBank.Services.Contracts.Common.Commands;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Processing.Commands
{
    public class PersonalCardTransferCommand : ICardCommand, ISecurityCodeCommand
    {
        public Guid FromCardId { get; set; }

        public Guid ToCardId { get; set; }

        public decimal Amount { get; set; }

        public SecurityCodeModel SecurityCode { get; set; }
    }
}
