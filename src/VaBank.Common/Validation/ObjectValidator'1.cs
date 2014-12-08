using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace VaBank.Common.Validation
{
    public class ObjectValidator<T> : AbstractValidator<T>, IObjectValidator<T>
    {
        IList<ValidationFault> IObjectValidator<T>.Validate(T obj)
        {
            return base.Validate((T)obj).Errors.Select(x => x.ToValidationFault()).ToList();
        }

        public Type ValidatedType
        {
            get { return typeof (T); }
        }

        public IList<ValidationFault> Validate(object obj, object state = null)
        {
            return base.Validate((T) obj).Errors.Select(x => x.ToValidationFault()).ToList();
        }
    }
}
