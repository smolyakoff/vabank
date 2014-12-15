using FluentValidation.Results;
using VaBank.Core.Membership.Entities;
using VaBank.Services.Contracts.Common.Queries;

namespace VaBank.Services.Common.Security
{
    public class UserQueryValidator : SecurityValidator<IUserQuery>
    {
        public UserQueryValidator(VaBankIdentity identity) : base(identity)
        {
            Inherit(new AuthenticatedSecurityValidator(identity));
            Custom(IsSecure);
        }

        private ValidationFailure IsSecure(IUserQuery query)
        {
            if (Identity.IsInRole(UserClaim.Roles.Customer) && query.UserId != Identity.UserId)
            {
                return new ValidationFailure(RootPropertyName, Messages.InsufficientRights);
            }
            return null;
        }
    }
}
