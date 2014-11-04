using System.Linq;
using FluentValidation.Results;
using VaBank.Common.Validation;
using VaBank.Core.Membership.Entities;

namespace VaBank.Services.Common.Security
{
    public class RoleSecurityValidator<T> : AuthenticatedSecurityValidator<T>
    {
        private readonly string[] _roles;

        public RoleSecurityValidator(VaBankIdentity identity, params string[] roles)
            :base(identity)
        {
            Argument.NotNull(roles, "roles");
            _roles = roles;
            Custom(HasRole);
        }

        private ValidationFailure HasRole(T context)
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
        public static RoleSecurityValidator<T> AdminOnly<T>(T context, VaBankIdentity identity)
        {
            return new RoleSecurityValidator<T>(identity, UserClaim.Roles.Admin);
        }

        public static RoleSecurityValidator<T> CustomerOnly<T>(T context, VaBankIdentity identity)
        {
            return new RoleSecurityValidator<T>(identity, UserClaim.Roles.Customer);
        }
    }
}
