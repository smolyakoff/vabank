using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace VaBank.Common.Validation
{
    public class AlwaysTrueValidator<T> : AbstractValidator<T>
    {
        public override ValidationResult Validate(T instance)
        {
            return new ValidationResult();
        }

        public override ValidationResult Validate(ValidationContext<T> context)
        {
            return new ValidationResult();
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<T> context)
        {
            return await Task.Run(() => new ValidationResult());
        }
    }
}
