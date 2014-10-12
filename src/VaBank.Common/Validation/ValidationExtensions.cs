using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace VaBank.Common.Validation
{
    public static class ValidationExtensions
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

        public static IRuleBuilderOptions<TContainer, TProperty> UseValidator<TContainer, TProperty>(
            this IRuleBuilderOptions<TContainer, TProperty> options, ObjectValidator<TProperty> validator)
        {
            return validator.Validate(options);
        }

        public static IRuleBuilderOptions<TContainer, TProperty> UseValidator<TContainer, TProperty>(
            this IRuleBuilderInitial<TContainer, TProperty> options, ObjectValidator<TProperty> validator)
        {
            return validator.Validate(options.Must(x => true));
        }
    }
}
