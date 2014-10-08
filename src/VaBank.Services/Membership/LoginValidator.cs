using FluentValidation;
using VaBank.Services.Common.Validation;

namespace VaBank.Services.Membership
{
    [ValidatorName("login")]
    [StaticValidator]
    public class LoginValidator : ObjectValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.NotEmpty().Length(4, 50);
        }
    }
}
