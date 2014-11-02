using System.Linq;
using System.Threading;
using FluentValidation;
using VaBank.Common.Validation;

namespace VaBank.Services.Common.Security
{
    public class RoleSecurityValidator : AbstractValidator<object>
    {
        public RoleSecurityValidator(params string[] roles)
        {
            Argument.NotNull(roles, "roles");
            var principal = Thread.CurrentPrincipal;

            RuleFor(x => x).Must(o => roles.Any(principal.IsInRole));
        }
    }
}
