using System;
using VaBank.Services.Contracts.Common.Commands;

namespace VaBank.Services.Contracts.Accounting.Commands
{
    public class CreateCardAccountCommand : IUserCommand
    {
        public Guid UserId { get; set; }

        public DateTime AccountExpirationDateUtc { get; set; }

        public string CardVendorId { get; set; }

        public string CardholderFirstName { get; set; }

        public string CardholderLastName { get; set; }

        public DateTime CardExpirationDateUtc { get; set; }

        public string CurrencyISOName { get; set; }

        public decimal InitialBalance { get; set; }
    }
}
