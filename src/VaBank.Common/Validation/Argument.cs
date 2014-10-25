using System;
using System.Linq;
using VaBank.Common.Validation.Validators;

namespace VaBank.Common.Validation
{
    public class Argument
    {
        public static void EnsureIsValid<TValidator, T>(T argument, string argumentName)
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
                throw new ArgumentException(message, argumentName);
            }
        }

        public static void EnsureIsValid<T>(T argument, IObjectValidator<T> validator, string argumentName)
        {
            if (string.IsNullOrEmpty(argumentName))
            {
                throw new ArgumentNullException("argumentName");
            }
            if (validator == null)
            {
                throw new ArgumentNullException("validator");
            }
            var faults = validator.Validate(argument);
            if (faults.Count > 0)
            {
                var message = string.Join(" ", faults.Select(x => x.Message));
                throw new ArgumentException(message, argumentName);
            }
        }

        public static void NotNull<T>(T argument, string argumentName)
            where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void NotEmpty(string argument, string argumentName)
        {
            EnsureIsValid<NotEmptyValidator, string>(argument, argumentName);
        }

        public T SafeSet<T, TValidator>(T value, out T field)
            where TValidator : IObjectValidator<T>, new()
        {
            EnsureIsValid<TValidator, T>(value, "value");
            field = value;
            return field;
        }
    }
}
