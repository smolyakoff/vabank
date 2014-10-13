using System;
namespace VaBank.Services.Contracts.Common.Security.Rules
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class SecurityRuleAttribute : Attribute
    {
        private readonly Type _ruleType;

        public SecurityRuleAttribute(Type ruleType)
        {
            if (ruleType == null)
                throw new ArgumentNullException("ruleType");
            if (!typeof(ISecurityRule).IsAssignableFrom(ruleType))
                throw new InvalidCastException("Can't cast ruleType to ISecurityRule.");
            _ruleType = ruleType;
        }
    }
}
