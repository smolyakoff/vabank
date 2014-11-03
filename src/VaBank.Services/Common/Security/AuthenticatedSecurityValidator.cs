using FluentValidation.Results;

namespace VaBank.Services.Common.Security
{
    public class AuthenticatedSecurityValidator<T> : SecurityValidator<T>
    {
        public AuthenticatedSecurityValidator(VaBankIdentity identity) : base(identity)
        {
            Custom(IsAuthenticated);
        }

        private ValidationFailure IsAuthenticated(T context)
        {
            return Identity.IsAuthenticated 
                ? null 
                : new ValidationFailure("{root}", RootPropertyName);
        }
    }
}
