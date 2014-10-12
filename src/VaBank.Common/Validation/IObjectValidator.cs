using System;
using System.Collections.Generic;

namespace VaBank.Common.Validation
{
    public interface IObjectValidator
    {
        bool CanValidate(Type type);

        Type ValidatedType { get; }

        IList<ValidationFault> Validate(object obj);
    }
}
