using System.Collections.Generic;
using VaBank.Services.Contracts.Common.Validation;

namespace VaBank.Services.Common.Validation
{
    public interface IObjectValidator<in T> : IObjectValidator
    {
        IList<ValidationFault> Validate(T obj);
    }
}
