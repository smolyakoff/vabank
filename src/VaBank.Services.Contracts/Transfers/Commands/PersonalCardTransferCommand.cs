using System;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Processing.Commands;

namespace VaBank.Services.Contracts.Transfers.Commands
{
    public class PersonalCardTransferCommand : ICardWithdrawalCommand
    {
        public Guid FromCardId { get; set; }

        public Guid ToCardId { get; set; }

        public decimal Amount { get; set; }

        public SecurityCodeModel SecurityCode { get; set; }
    }
}
