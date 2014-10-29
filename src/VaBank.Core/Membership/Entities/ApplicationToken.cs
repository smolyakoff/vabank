using System;
using VaBank.Core.Common;

namespace VaBank.Core.Membership.Entities
{
    public class ApplicationToken : Entity<string>
    {
        public virtual ApplicationClient Client { get; set; }
        public virtual User User { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
    }
}