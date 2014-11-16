using System;
using FluentValidation;
using VaBank.Common.Validation;

namespace VaBank.Core.Common
{
    public class FutureDateValidator : ObjectValidator<DateTime>
    {
        public override IRuleBuilderOptions<TContainer, DateTime> Validate<TContainer>(IRuleBuilderOptions<TContainer, DateTime> builder)
        {
            return builder.GreaterThan(x => DateTime.UtcNow);
        }
    }
}
