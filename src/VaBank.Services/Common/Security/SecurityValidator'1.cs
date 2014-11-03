using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Common.Security;

namespace VaBank.Services.Common.Security
{
    public abstract class SecurityValidator<T> : AbstractValidator<T>, ISecurityValidator<T>
    {
        private readonly VaBankIdentity _identity;

        protected const string RootPropertyName = "{root}";

        protected SecurityValidator(VaBankIdentity identity)
        {
            Argument.NotNull(identity, "identity");
            _identity = identity;
        } 

        protected virtual void Inherit(ISecurityValidator<T> validator, string propertyName = "{root}")
        {
            Argument.NotNull(validator, "validator");
            Custom(o =>
            {
                var validationResult = validator.Validate(o);
                if (validationResult.IsValid)
                {
                    return null;
                }
                var error = string.Join(Environment.NewLine, validationResult.Errors.Select(x => x.ErrorMessage));
                return new ValidationFailure(propertyName, error);
            });
        }

        protected VaBankIdentity Identity
        {
            get { return _identity; }
        }
    }
}
