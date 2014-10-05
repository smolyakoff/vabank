using System;
using System.Collections.Generic;
using VaBank.Services.Contracts.Common.Validation;

namespace VaBank.Services.Common.Validation
{
    public abstract class ObjectValidator<T> : IObjectValidator<T>
    {
        public abstract IList<ValidationFault> Validate(T obj);

        public bool CanValidate(Type type)
        {
            return typeof (T).IsAssignableFrom(type);
        }

        public Type ValidatedType
        {
            get { return typeof (T); }
        }

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

        protected class Container
        {
            public Container(T obj)
            {
                Value = obj;
            }

            public T Value { get; set; }
        }
    }
}
