using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using VaBank.Services.Contracts.Common.Validation;

namespace VaBank.Services.Common.Validation
{
    internal static class ValidationExtensions
    {
        public static ValidationFault ToValidationFault(this ValidationFailure failure, string propertyNameOverride = null)
        {
            if (failure == null)
            {
                throw new ArgumentNullException("failure");
            }
            return new ValidationFault(string.IsNullOrEmpty(propertyNameOverride) ? failure.PropertyName : propertyNameOverride, failure.ErrorMessage);
        }

        public static IList<ValidationFault> ToValidationFaults(this IEnumerable<ValidationFailure> failures, string propertyNameOverride = null)
        {
            if (failures == null)
            {
                throw new ArgumentNullException("failures");
            }
            return failures.Select(x => ToValidationFault(x, propertyNameOverride)).ToList();
        }
    }
}
