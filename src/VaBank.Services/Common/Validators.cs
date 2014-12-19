using System;
using FluentValidation;
using VaBank.Services.Contracts.Common.Queries;

namespace VaBank.Services.Common
{
    public class RangeQueryValidator : AbstractValidator<IRangeQuery<DateTime>>
    {
        public RangeQueryValidator()
        {
            RuleFor(x => x.From).Must((query, from) => from <= query.To)
                .WithLocalizedMessage(() => "From value can't be bigger than To value.");
        }
    }
}
