using System;
using System.Collections.Generic;
using FluentValidation;

namespace VaBank.Common.Validation
{
    public abstract class ObjectValidator<T> : IObjectValidator<T>
    {
        public virtual IList<ValidationFault> Validate(T obj)
        {
            var container = new Container(obj);
            var validator = new InlineValidator<Container>();
            var builder = Validate(validator.RuleFor(x => x.Value).Must(x => true));
            var name = OverrideName();
            if (!string.IsNullOrEmpty(name))
            {
                builder.WithName(name);
            }
            return validator.Validate(container).Errors.ToValidationFaults();
        }

        protected virtual string OverrideName()
        {
            return string.Empty;
        }

        public bool CanValidate(Type type)
        {
            return typeof (T).IsAssignableFrom(type);
        }

        public Type ValidatedType
        {
            get { return typeof (T); }
        }

        public abstract IRuleBuilderOptions<TContainer, T> Validate<TContainer>(IRuleBuilderOptions<TContainer, T> builder);

        protected ValidationFault Fault(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }
            return new ValidationFault("o", message);
        }

        IList<ValidationFault> IObjectValidator.Validate(object obj)
        {
            if (obj == null)
            {
                return Validate(default(T));
            }
            if (!CanValidate(obj.GetType()))
            {
                var message = string.Format("Can't validate object of type {0}.", obj.GetType());
                throw new NotSupportedException(message);
            }
            return Validate((T) obj);
        }

        private class Container
        {
            public Container(T obj)
            {
                Value = obj;
            }

            public T Value { get; private set; }
        }
    }
}
