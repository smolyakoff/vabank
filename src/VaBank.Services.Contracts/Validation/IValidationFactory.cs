using FluentValidation;

namespace VaBank.Services.Contracts.Validation
{
    public interface IValidationFactory
    {
        AbstractValidator<T> GetValidator<T>();
    }
}
