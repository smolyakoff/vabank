using FluentValidation;

namespace VaBank.Services.Contracts.Common.Validation
{
    public interface ISecurityValidator<in T> : IValidator<T>
    {
    }
}
