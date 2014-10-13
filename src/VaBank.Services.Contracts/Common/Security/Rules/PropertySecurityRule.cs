using System;
using System.Collections.Generic;

namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public class PropertySecurityRule<TProperty> : IPropertySecurityRule<TProperty>
    {
        private readonly List<IPropertySecurityRule<TProperty>> _rules;

        public PropertySecurityRule()
        {
            _rules = new List<IPropertySecurityRule<TProperty>>();
        }

        public IPropertySecurityRule<TProperty> SetRule(IPropertySecurityRule<TProperty> rule)
        {
            if (rule == null)
                throw new ArgumentNullException("rule");
            _rules.Add(rule);
            return this;
        }

        public SecurityRuleMatchResult Match(TProperty obj)
        {
            var faults = new List<SecurityRuleFault>();
            foreach (var rule in _rules)
            {
                var matchResult = rule.Match(obj);
                if (!matchResult.IsMatch)
                {
                    faults.AddRange(matchResult.Faults);
                    return new SecurityRuleMatchResult(faults);
                }
            }

            return new SecurityRuleMatchResult(faults);
        }

        SecurityRuleMatchResult ISecurityRule.Match(object obj)
        {
            return Match((TProperty)obj);
        }

        public string SecurityMessage
        {
            get { return null; }
        }
    }
}
