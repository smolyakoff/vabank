using FluentValidation;
using FluentValidation.Results;
using VaBank.Services.Common.Validation;
using VaBank.Services.Contracts.Membership;

namespace VaBank.Services.Membership
{
    [StaticValidator]
    internal class LoginCommandValidator : AbstractValidator<LoginCommand>
    {        
        public LoginCommandValidator()
        {
            //TODO: Login validator attachment
            RuleFor(x => x.Password).NotEmpty().Length(6, 256);
        }
    }

    internal class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
    {
        public CreateTokenCommandValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().Length(1, 256);
            RuleFor(x => x.Id).NotEmpty().Length(1, 256);
            Custom(x =>
                {
                    return x.ExpireUtc >= x.IssuedUtc ? null :
                        new ValidationFailure("ExpireUtc", "ExpreUtc value can't be less than IssuedUtc value.");
                });
            RuleFor(x => x.ProtectedTicket).NotEmpty().Length(1, 256);
            RuleFor(x => x.Subject).NotEmpty().Length(1, 50);
        }
    }
}
