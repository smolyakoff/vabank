using System;

namespace VaBank.Core.Entities.Membership
{
    public class ApplicationToken : Entity<Guid>
    {
        public virtual ApplicationClient Client { get; set; }
        public Guid ClientId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
    }
}
