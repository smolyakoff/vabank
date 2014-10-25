using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentValidation;
using VaBank.Common.Resources;
using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Core.Membership.Entities;
using VaBank.Core.Membership.Resources;

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
        private static readonly HashSet<string> WorstPasswords;

        static PasswordValidator()
        {
            WorstPasswords = new HashSet<string>(
                EmbeddedResource.ReadAsString(typeof(Entity).Assembly, "Membership/Resources/Top_500_Worst_Passwords.txt")
                .Split(new [] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .Distinct());
        }

        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder
                .NotEmpty().WithLocalizedMessage(() => Messages.NotEmpty)
                .Length(6, 256).WithLocalizedMessage(() => Messages.FieldLength, 6)
                .Matches(@"\d").WithLocalizedMessage(() => Messages.PasswordFormatNumber)
                .Matches(@"\!|@|#|\$|%|\^|\&|'").WithLocalizedMessage(() => Messages.PasswordFormatSpecChar)
                .Must(x => !WorstPasswords.Contains(x)).WithLocalizedMessage(() => Messages.WeakPassword);
        }
    }

    [ValidatorName("phone")]
    [StaticValidator]
    public class PhoneNumberValidator : ObjectValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.Matches(@"\+375 *\(?(29|33|44|25)\)? *\d{7}$").WithLocalizedMessage(() => Messages.CheckNumberPhone);
        }
    }

    [ValidatorName("role")]
    [StaticValidator]
    public class RoleValidator : ObjectValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.Must(UserClaim.IsSupportedRole).WithLocalizedMessage(() => Messages.InvalidRole);
        }
    }
}
