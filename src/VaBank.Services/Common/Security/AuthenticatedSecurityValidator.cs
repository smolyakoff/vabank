using FluentValidation.Results;

namespace VaBank.Services.Common.Security
{
    public class AuthenticatedSecurityValidator : SecurityValidator<object>
    {
        public AuthenticatedSecurityValidator(VaBankIdentity identity) : base(identity)
        {
            Custom(IsAuthenticated);
        }

        private ValidationFailure IsAuthenticated(object context)
        {
            return Identity.IsAuthenticated 
                ? null 
                : new ValidationFailure("{root}", Messages.NotAuthenticated);
        }
    }
}
