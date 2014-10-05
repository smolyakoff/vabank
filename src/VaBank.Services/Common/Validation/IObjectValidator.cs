using System;
using System.Collections.Generic;
using VaBank.Services.Contracts.Common.Validation;

namespace VaBank.Services.Common.Validation
{
    public interface IObjectValidator
    {
        bool CanValidate(Type type);

        Type ValidatedType { get; }

        IList<ValidationFault> Validate(object obj);
    }
}
