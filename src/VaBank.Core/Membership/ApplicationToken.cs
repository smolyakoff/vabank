using System;
using VaBank.Core.Common;

namespace VaBank.Core.Membership
{
    public class ApplicationToken : Entity<string>
    {
        public virtual ApplicationClient Client { get; set; }
        public string ClientId { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
        public string Subject { get; set; }
    }
}