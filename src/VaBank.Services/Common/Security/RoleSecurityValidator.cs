using System.Linq;
using FluentValidation.Results;
using VaBank.Common.Validation;
using VaBank.Core.Membership.Entities;

namespace VaBank.Services.Common.Security
{
    public class RoleSecurityValidator : AuthenticatedSecurityValidator
    {
        private readonly string[] _roles;

        public RoleSecurityValidator(VaBankIdentity identity, params string[] roles)
            :base(identity)
        {
            Argument.NotNull(roles, "roles");
            _roles = roles;
            Custom(HasRole);
        }

        private ValidationFailure HasRole(object context)
        {
            if (!_roles.Any(Identity.IsInRole))
            {
                return new ValidationFailure("{root}", Messages.InsufficientRights);
            }
            return null;
        }
    }

    public static class RoleSecurity
    {
        public static RoleSecurityValidator AdminOnly(VaBankIdentity identity)
        {
            return new RoleSecurityValidator(identity, UserClaim.Roles.Admin);
        }

        public static RoleSecurityValidator CustomerOnly(VaBankIdentity identity)
        {
            return new RoleSecurityValidator(identity, UserClaim.Roles.Customer);
        }
    }
}
