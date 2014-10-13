using System.Reflection;

namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public interface IPropertySecurityRule<TProperty> : ISecurityRule<TProperty>
    {
        string SecurityMessage { get; }
    }
}
