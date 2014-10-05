using FluentValidation;
using VaBank.Services.Contracts.Maintenance;

namespace VaBank.Services.Maintenance
{
    internal class SystemLogQueryValidator : AbstractValidator<SystemLogQuery>
    {
        public SystemLogQueryValidator()
        {
            RuleFor(x => x.Filter).NotNull();
        }
    }

    internal class SystemLogClearCommandValidator : AbstractValidator<SystemLogClearCommand>
    {
        public SystemLogClearCommandValidator()
        {
            RuleFor(x => x.Ids).NotNull();
        }
    }
}
