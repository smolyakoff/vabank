using System;

namespace VaBank.Services.Contracts.Membership
{
    public class TokenModel
    {
        public string Id { get; set; }
        public string ProtectedTicket { get; set; }
        public string ClientId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpireUtc { get; set; }
        public Guid UserId { get; set; }
    }
}
