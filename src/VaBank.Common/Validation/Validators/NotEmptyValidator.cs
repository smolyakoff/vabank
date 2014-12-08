using FluentValidation;

namespace VaBank.Common.Validation.Validators
{
    [StaticValidator]
    public class NotEmptyValidator : PropertyValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.NotEmpty();
        }
    }
}
