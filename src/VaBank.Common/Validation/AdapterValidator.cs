using System;
using System.Linq;
using FluentValidation.Validators;

namespace VaBank.Common.Validation
{
    internal class AdapterValidator<TInstance, T> : PropertyValidator
    {
        private readonly Func<TInstance, T> _selector;

        private readonly ObjectValidator<T> _validator;  

        public AdapterValidator(Func<TInstance, T> selector, ObjectValidator<T> validator) : base("{ValidationMessage}")
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            if (validator == null)
            {
                throw new ArgumentNullException("validator");
            }
            _selector = selector;
            _validator = validator;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var validatable = _selector((TInstance)context.Instance);
            var faults = _validator.Validate(validatable);
            if (faults.Count <= 0)
            {
                return true;
            }
            var message = string.Join(Environment.NewLine, faults.Select(x => x.Message).ToArray());
            context.MessageFormatter.AppendArgument("ValidationMessage", message);
            return false;
        }
    }
}
