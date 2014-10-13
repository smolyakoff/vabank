using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public class PropertySecurityRuleBuilder<T, TProperty> : IPropertySecurityRuleBuilder<T, TProperty>
    {       
        private readonly PropertySecurityRule<TProperty> _initial;

        public PropertySecurityRuleBuilder()
        {
            _initial = new PropertySecurityRule<TProperty>();
        }        

        public IPropertySecurityRuleBuilder<T, TProperty> Must(Expression<Func<TProperty, bool>> predicate)
        {
            _initial.SetRule(new ConstraintPropertySecurityRule<TProperty>(predicate));
            return this;
        }

        public IPropertySecurityRuleBuilder<T, TProperty> Equal(TProperty value) 
        {
            _initial.SetRule(new EqualPropertySecurityRule<TProperty>(value));
            return this;
        }

        public IPropertySecurityRuleBuilder<T, TProperty> NotEqual(TProperty value)
        {
            _initial.SetRule(new NotEqualPropertySecurityRule<TProperty>(value));
            return this;
        }

        public IPropertySecurityRule<TProperty> GetRule()
        {            
            return _initial;
        }

        ISecurityRule ISecurityRuleBuilder.GetRule()
        {
            return GetRule();
        }

        public IPropertySecurityRuleBuilder<T, TProperty> SetRule(IPropertySecurityRule<TProperty> rule)
        {
            _initial.SetRule(rule);
            return this;
        }
    }
}
