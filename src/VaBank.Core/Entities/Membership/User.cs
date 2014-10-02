using System;
using System.Collections.Generic;

namespace VaBank.Core.Entities.Membership
{
    public class User : Entity<Guid>
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string SecurityStamp { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime LockoutEndDateUtc { get; set; }
        public string UserName { get; set; }
        public int AccessFailedCount { get; set; }
        public bool Deleted { get; set; }
        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual UserProfile Profile { get; set; }
    }
}
