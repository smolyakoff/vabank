using System;
using System.Linq;
using VaBank.Common.Validation;

namespace VaBank.Core.Common
{
    public class Entity
    {
        internal static void EnsureArgumentValid<TValidator, T>(T argument, string argumentName)
            where TValidator : IObjectValidator<T>, new()
        {
            if (string.IsNullOrEmpty(argumentName))
            {
                throw new ArgumentNullException("argumentName");
            }
            var validator = new TValidator();
            var faults = validator.Validate(argument);
            if (faults.Count > 0)
            {
                var message = string.Join(" ", faults.Select(x => x.Message));
                throw new ArgumentException(argumentName, argumentName);
            }
        } 
    }
}