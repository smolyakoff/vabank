using System.Collections.Generic;
using FluentValidation.Results;
using VaBank.Services.Contracts.Common.Security;

namespace VaBank.Services.Common.Security
{
    public class SecurityValidatorException : AccessDeniedException
    {
        internal SecurityValidatorException(IEnumerable<ValidationFailure> failures) 
            : base(new MethodCallDenied(failures))
        {
        }
    }
}
