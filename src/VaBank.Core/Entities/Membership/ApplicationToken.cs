using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Core.Entities.Membership
{
    public class ApplicationToken : Entity<Guid>
    {
        public virtual ApplicationClient Client { get; set; }
        public Guid ClientID { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
    }
}
