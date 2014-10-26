using System;
using FluentValidation;
using VaBank.Common.Validation;

namespace VaBank.Core.Accounting
{
    [ValidatorName("cardExpirationDate")]
    [StaticValidator]
    public class CardExpirationDateValidator : ObjectValidator<DateTime>
    {
        public override IRuleBuilderOptions<TContainer, DateTime> Validate<TContainer>(IRuleBuilderOptions<TContainer, DateTime> builder)
        {
            var now = DateTime.UtcNow.Date;
            var maxDate = now.AddYears(5);
            return builder.InclusiveBetween(now, maxDate);
        }
    }
}
