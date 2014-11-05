using System;

namespace VaBank.Services.Contracts.Infrastructure.Secuirty
{
    public class SecurityCodeTicketModel
    {
        public Guid Id { get; set; }

        public DateTime ExpirationDateUtc { get; set; }
    }
}
