using System;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Processing.Commands;

namespace VaBank.Services.Contracts.Transfers.Commands
{
    public class InterbankCardTransferCommand : ICardWithdrawalCommand
    {
        public Guid FromCardId { get; set; }

        public string ToCardNo { get; set; }

        public DateTime ToCardExpirationDateUtc { get; set; }

        public SecurityCodeModel SecurityCode { get; set; }

        public decimal Amount { get; set; }
    }
}
