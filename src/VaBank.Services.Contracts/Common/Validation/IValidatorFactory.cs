using FluentValidation;

namespace VaBank.Services.Contracts.Common.Validation
{
    public interface IValidatorFactory
    {
        IValidator<T> GetValidator<T>();
    }
}
