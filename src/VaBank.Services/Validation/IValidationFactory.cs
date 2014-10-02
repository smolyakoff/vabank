using FluentValidation;

namespace VaBank.Services.Validation
{
    public interface IValidationFactory
    {
        AbstractValidator<T> GetValidator<T>();
    }
}
