using FluentValidation;
using VaBank.Services.Common.Validation;
using VaBank.Services.Contracts.Maintenance;

namespace VaBank.Services.Maintenance
{
    [StaticValidator]
    internal class SystemLogQueryValidator : AbstractValidator<SystemLogQuery>
    {
        public SystemLogQueryValidator()
        {
            RuleFor(x => x.ClientFilter).NotNull();
        }
    }

    [StaticValidator]
    internal class SystemLogClearCommandValidator : AbstractValidator<SystemLogClearCommand>
    {
        public SystemLogClearCommandValidator()
        {
            RuleFor(x => x.Ids).NotNull();
        }
    }
}
