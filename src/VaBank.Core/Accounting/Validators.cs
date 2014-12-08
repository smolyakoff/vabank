using System;
using FluentValidation;
using VaBank.Common.Validation;

namespace VaBank.Core.Accounting
{
    [ValidatorName("cardExpirationDate")]
    [StaticValidator]
    public class CardExpirationDateValidator : PropertyValidator<DateTime>
    {
        public override IRuleBuilderOptions<TContainer, DateTime> Validate<TContainer>(IRuleBuilderOptions<TContainer, DateTime> builder)
        {
            var now = DateTime.UtcNow.Date;
            var maxDate = now.AddYears(5);
            return builder.InclusiveBetween(now, maxDate);
        }
    }

    [StaticValidator]
    [ValidatorName("accountNumber")]
    public class AccountNumberValidator : PropertyValidator<string>
    {
        public override IRuleBuilderOptions<TContainer, string> Validate<TContainer>(IRuleBuilderOptions<TContainer, string> builder)
        {
            return builder.Length(13);
        }
    }
}
