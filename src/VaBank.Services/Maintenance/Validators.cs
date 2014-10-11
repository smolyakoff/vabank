using FluentValidation;
using VaBank.Services.Common.Validation;
using VaBank.Services.Contracts.Maintenance;
using VaBank.Services.Contracts.Maintenance.Commands;
using VaBank.Services.Contracts.Maintenance.Queries;

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
