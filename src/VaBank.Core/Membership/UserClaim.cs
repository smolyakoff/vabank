using System;
using System.Collections.Generic;
using System.Security.Claims;
using VaBank.Core.Common;

namespace VaBank.Core.Membership
{
    public class UserClaim : Entity
    {
        public static class Role
        {
            private const string AdminRole = "Admin";

            private const string CustomerRole = "Customer";

            public static readonly List<string> RoleNames = new List<string> {AdminRole, CustomerRole};

            public static UserClaim Create(Guid userId, string roleName)
            {
                if (!RoleNames.Contains(roleName))
                {
                    throw new ArgumentException("Role name is invalid");
                }
                return new UserClaim {UserId = userId, Type = ClaimTypes.Role, Value = roleName};
            }
        }

        public Guid UserId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}