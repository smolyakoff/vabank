using FluentValidation.Results;
using VaBank.Core.Membership.Entities;
using VaBank.Services.Contracts.Common.Commands;

namespace VaBank.Services.Common.Security
{
    public class UserCommandValidator : SecurityValidator<IUserCommand>
    {
        public UserCommandValidator(VaBankIdentity identity) : base(identity)
        {
            Custom(IsSecure);
        }

        private ValidationFailure IsSecure(IUserCommand command)
        {
            if (Identity.UserId.HasValue && Identity.IsInRole(UserClaim.Roles.Customer) && command.UserId != Identity.UserId)
            {
                return new ValidationFailure(RootPropertyName, Messages.InsufficientRights);
            }
            return null;
        }
    }
}
