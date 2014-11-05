using FluentValidation;

namespace VaBank.Services.Contracts.Common.Security
{
    public interface ISecurityValidator<in T> : IValidator<T>
    {
    }
}
