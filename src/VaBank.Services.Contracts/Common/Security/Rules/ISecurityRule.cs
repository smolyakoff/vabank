namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public interface ISecurityRule
    {
        SecurityRuleMatchResult Match(object obj);
    }
}
