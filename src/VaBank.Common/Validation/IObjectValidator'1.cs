using System.Collections.Generic;

namespace VaBank.Common.Validation
{
    public interface IObjectValidator<in T> : IObjectValidator
    {
        IList<ValidationFault> Validate(T obj);
    }
}
