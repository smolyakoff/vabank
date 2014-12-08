using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using VaBank.Common.Validation.Validators;

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
            this IRuleBuilderOptions<TContainer, TProperty> options, PropertyValidator<TProperty> validator)
        {
            if (validator == null)
            {
                throw new ArgumentNullException("validator");
            }
            return validator.Validate(options);
        }

        public static IRuleBuilderOptions<TContainer, TProperty> UseValidator<TContainer, TProperty>(
            this IRuleBuilderInitial<TContainer, TProperty> options, PropertyValidator<TProperty> validator)
        {
            if (validator == null)
            {
                throw new ArgumentNullException();
            }
            return validator.Validate(options.Must(x => true));
        }

        public static IRuleBuilderOptions<TContainer, TProperty> UseValidator<TContainer, TProperty, T>(
            this IRuleBuilderInitial<TContainer, TProperty> options, Func<TContainer, T> selector, PropertyValidator<T> validator)
        {
            return options.SetValidator(new AdapterValidator<TContainer, T>(selector, validator));
        }

        public static IRuleBuilderOptions<TContainer, TProperty> UseValidator<TContainer, TProperty, T>(
            this IRuleBuilderOptions<TContainer, TProperty> options, Func<TContainer, T> selector, PropertyValidator<T> validator)
        {
            return options.SetValidator(new AdapterValidator<TContainer, T>(selector, validator));
        }
    }
}
