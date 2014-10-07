using System.Collections.Generic;
using FluentValidation;
using VaBank.Services.Common.Validation;
using VaBank.Services.Contracts.Common.Validation;

namespace VaBank.Services.Membership
{
    [ValidatorName("login")]
    [StaticValidator]
    public class LoginValidator : ObjectValidator<string>
    {
        public override IList<ValidationFault> Validate(string obj)
        {
            var value = new Container(obj);
            var inline = new InlineValidator<Container>();
            inline.RuleFor(x => x.Value)
                .NotEmpty()
                .Length(6, 20)
                .WithName("Login");
            return inline.Validate(value).Errors.ToValidationFaults("login");
        }
    }
}
