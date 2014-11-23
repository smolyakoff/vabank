using FluentValidation.Results;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.App.Entities;
using VaBank.Services.Contracts.Common.Commands;

namespace VaBank.Services.Common.Security
{
    public class CodeSecurityValidator : SecurityValidator<ISecurityCodeCommand>
    {
        private readonly IRepository<SecurityCode> _securityCodeRepository;

        public CodeSecurityValidator(VaBankIdentity identity, IRepository<SecurityCode> securityCodeRepository) : base(identity)
        {
            Argument.NotNull(securityCodeRepository, "securityCodeRepository");

            _securityCodeRepository = securityCodeRepository;
            Inherit(new AuthenticatedSecurityValidator(identity));
            Custom(IsSecure);
        }

        private ValidationFailure IsSecure(ISecurityCodeCommand command)
        {
            var shouldValidateSecurityCode = Identity.User.Profile != null && Identity.User.Profile.SmsConfirmationEnabled;
            if (!shouldValidateSecurityCode)
            {
                return null;
            }
            if (command.SecurityCode == null)
            {
                return new ValidationFailure(RootPropertyName, Messages.MissingSecurityCode);
            }
            var securityCode = _securityCodeRepository.Find(command.SecurityCode.Id);
            if (securityCode == null)
            {
                return new ValidationFailure(RootPropertyName, Messages.InvalidSecurityCode);
            }
            var valid = securityCode.Deactivate(command.SecurityCode.Code);
            return valid ? null : new ValidationFailure(RootPropertyName, Messages.InvalidSecurityCode);
        }
    }
}
