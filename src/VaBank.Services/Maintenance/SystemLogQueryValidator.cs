using FluentValidation;
using VaBank.Services.Contracts.Admin.Maintenance;

namespace VaBank.Services.Maintenance
{
    internal class SystemLogQueryValidator : AbstractValidator<SystemLogQuery>
    {
        public SystemLogQueryValidator()
        {
            RuleFor(x => x.Filter).NotNull();
        }
    }
}
