using System;

namespace VaBank.Core.Entities.Membership
{
    public class UserClaim : Entity<Guid>
    {
        public string Type { get; set; }
        public string Value { get; set; }        
    }
}
