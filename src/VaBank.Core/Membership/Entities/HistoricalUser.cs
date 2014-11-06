using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Core.Common;

namespace VaBank.Core.Membership.Entities
{
    public class HistoricalUser : HistoricalEntity, IUser
    {
        public string PasswordHash { get; private set; }
        public string PasswordSalt { get; private set; }
        public bool LockoutEnabled { get; private set; }
        public DateTime? LockoutEndDateUtc { get; private set; }
        public string UserName { get; private set; }
        public int AccessFailedCount { get; private set; }
        public bool Deleted { get; private set; }
        public Guid Id { get; private set; }
    }
}
