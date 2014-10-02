using FluentValidation;
using VaBank.Services.Contracts.Admin.Maintenance;

namespace VaBank.Services.Validation.Validators
{
    public class SystemLogQueryValidator : AbstractValidator<SystemLogQuery>
    {
        public SystemLogQueryValidator()
        {
            RuleFor(x => x.Filter).NotNull();
        }
    }
}
