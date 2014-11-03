using System;

namespace VaBank.Services.Contracts.Infrastructure
{
    public class SecurityCodeTicketModel
    {
        public Guid Id { get; set; }

        public DateTime ExpirationDateUtc { get; set; }
    }
}
