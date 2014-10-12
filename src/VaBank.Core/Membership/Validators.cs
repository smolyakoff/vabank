using FluentValidation;
using VaBank.Common.Validation;

namespace VaBank.Core.Membership
{
    [ValidatorName("userName")]
    [StaticValidator]
    public class UserNameValidator : ObjectValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty).Length(4, 50)
                .WithLocalizedMessage(() => Messages.FieldLength, 4);
        }
    }

    [ValidatorName("password")]
    [StaticValidator]
    public class PasswordValidator : ObjectValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty).Length(6, 256)
                .WithLocalizedMessage(() => Messages.FieldLength, 6);
        }
    }

    [ValidatorName("phone")]
    [StaticValidator]
    public class PhoneNumberValidator : ObjectValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.Matches(@"\+375 *\(?(29|33|44|25)\)? *\d{7}").WithLocalizedMessage(() => Messages.CheckNumberPhone);
        }
    }

    [ValidatorName("role")]
    [StaticValidator]
    public class RoleValidator : ObjectValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.Must(UserClaim.Role.RoleNames.Contains).WithLocalizedMessage(() => Messages.InvalidRole);
        }
    }
}
