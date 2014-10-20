using System;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Common.Commands;

namespace VaBank.Services.Contracts.Accounting.Commands
{
    public class CreateCardAccountCommand : IUserCommand
    {
        public Guid UserId { get; set; }

        public string AccountNo { get; set; }

        public DateTime AccountExpirationDateUtc
        {
            get { return CardExpirationDateUtc; }
        }

        public string CardNo { get; set; }

        public string CardVendorId { get; set; }

        public string CardholderFirstName { get; set; }

        public string CardholderLastName { get; set; }

        public DateTime CardExpirationDateUtc { get; set; }

        public string CurrencyISOCode { get; set; }

        public decimal InitialBalance { get; set; }

        public CardLimitsModel CardLimits { get; set; }
    }
}
