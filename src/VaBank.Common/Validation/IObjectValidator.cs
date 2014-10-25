using System;
using System.Collections.Generic;

namespace VaBank.Common.Validation
{
    public interface IObjectValidator
    {
        Type ValidatedType { get; }

        IList<ValidationFault> Validate(object obj, object state = null);
    }
}
