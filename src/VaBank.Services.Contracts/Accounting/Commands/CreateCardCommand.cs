using System;

namespace VaBank.Services.Contracts.Accounting.Commands
{
    public class CreateCardCommand
    {
        public string AccountNo { get; set; }

        public string CardVendorId { get; set; }

        public string CardholderFirstName { get; set; }

        public string CardholderLastName { get; set; }

        public DateTime ExpirationDateUtc { get; set; }
    }
}
