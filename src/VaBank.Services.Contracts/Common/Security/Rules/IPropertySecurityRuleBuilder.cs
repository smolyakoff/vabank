namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public interface IPropertySecurityRuleBuilder<T, TProperty> : ISecurityRuleBuilder
    {
        IPropertySecurityRuleBuilder<T, TProperty> SetRule(IPropertySecurityRule<TProperty> rule);
        IPropertySecurityRule<TProperty> GetRule();
    }
}
