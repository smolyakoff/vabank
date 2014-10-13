namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public interface ISecurityRule<T> : ISecurityRule
    {
        SecurityRuleMatchResult Match(T obj);
    }
}
