using System;

namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public interface ISecurityRuleFactory
    {
        ISecurityRule GetRule(Type type);
    }
}
