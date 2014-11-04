using System;
using System.Collections.Generic;
using System.Security.Claims;
using VaBank.Core.Common;

namespace VaBank.Core.Membership.Entities
{
    public class UserClaim : Entity
    {
        private static readonly List<string> SupportedRoles = 
            new List<string> { Roles.Admin, Roles.Customer };

        public static class Types
        {
            public const string Role = ClaimTypes.Role;

            public const string UserId = ClaimTypes.Sid;

            public const string UserName = ClaimTypes.Name;

            public const string ClientId = "https://vabank.azurewebsites.net/api/claimTypes/clientId";
        }

        public static class Roles
        {
            public const string Admin = "Admin";

            public const string Customer = "Customer";
        }

        public static bool IsSupportedRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }
            return SupportedRoles.Contains(roleName);
        }

        public static UserClaim CreateRole(Guid userId, string roleName)
        {
            if (!SupportedRoles.Contains(roleName))
            {
                var message = string.Format("Role {0} is not supported.", roleName);
                throw new NotSupportedException(message);
            }
            return new UserClaim {UserId = userId, Type = ClaimTypes.Role, Value = roleName};
        }

        public Guid UserId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}