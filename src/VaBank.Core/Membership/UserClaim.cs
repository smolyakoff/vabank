using System;
using VaBank.Core.Common;

namespace VaBank.Core.Membership
{
    public class UserClaim : Entity
    {
        public Guid UserId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}