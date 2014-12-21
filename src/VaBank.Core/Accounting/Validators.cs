using System;
using FluentValidation;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Resources;

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
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            return builder.InclusiveBetween(now, maxDate)
                .WithLocalizedMessage(() => Messages.CardExpirationDateInvalid)
                .Must(IsLastDayOfAMonth)
                .WithLocalizedMessage(() => Messages.CardExpirationDateIsLastDay);
        }

        private bool IsLastDayOfAMonth(DateTime date)
        {
            var lastDayOfMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return date.Day == lastDayOfMonth;
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
