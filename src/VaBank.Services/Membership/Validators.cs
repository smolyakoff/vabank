using System;
using FluentValidation;
using VaBank.Services.Common.Validation;
using VaBank.Services.Contracts.Membership.Commands;

namespace VaBank.Services.Membership
{
    [ValidatorName("login")]
    [StaticValidator]
    internal class LoginValidator : ObjectValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.NotEmpty().Length(4, 50);
        }
    }

    [StaticValidator]
    internal class LoginCommandValidator : AbstractValidator<LoginCommand>
    {        
        public LoginCommandValidator()
        {
            RuleFor(x => x.Login).UseValidator(new LoginValidator());
            RuleFor(x => x.Password).NotEmpty().Length(6, 256);
        }
    }

    [StaticValidator]
    internal class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
    {
        public CreateTokenCommandValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().Length(1, 256);
            RuleFor(x => x.Id).NotEmpty().Length(1, 256);
            RuleFor(x => x.ExpiresUtc).Must((command, expireUtc) => expireUtc >= command.IssuedUtc)
                .WithLocalizedMessage(() => "ExpreUtc value can't be less than IssuedUtc value.");
            RuleFor(x => x.ProtectedTicket).NotEmpty().Length(1, 512);
            RuleFor(x => x.UserId).Must(x => x != Guid.Empty);
        }
    }
}
