using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Common.Security;

namespace VaBank.Services.Common.Security
{
    internal class MethodCallDenied : AccessDenied
    {
        private readonly UserMessage _userMessage;

        public MethodCallDenied(IEnumerable<ValidationFailure> failures)
        {
            var validationFailures = failures as IList<ValidationFailure> ?? failures.ToList();
            Argument.NotNull(validationFailures, "failures");
            _userMessage =  UserMessage.Format(
                string.Join(Environment.NewLine, validationFailures.Select(x => x.ErrorMessage)), 
                "SECURITY_FAULT");
        }

        public override UserMessage UserMessage
        {
            get { return _userMessage; }
        }
    }
}
