using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public class ConstraintPropertySecurityRule<TProperty> : IPropertySecurityRule<TProperty>
    {
        private const string DefaultMessage = "ConstraintPropertyRule security error. Constraint: {0}";
        
        protected Expression<Func<TProperty, bool>> Constraint;

        protected ConstraintPropertySecurityRule()
        {
        }

        public ConstraintPropertySecurityRule(Expression<Func<TProperty, bool>> constraint, string message)
        {
            if (constraint == null)
                throw new ArgumentNullException("constraint");
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(message);
            Constraint = constraint;
            SecurityMessage = message;
        }

        public ConstraintPropertySecurityRule(Expression<Func<TProperty, bool>> constraint) 
            : this(constraint, string.Format(DefaultMessage, constraint.ToString()))
        {
        }
        
        public SecurityRuleMatchResult Match(TProperty obj)
        {
            var faults = new List<SecurityRuleFault>();
            if (!Constraint.Compile()(obj))
                faults.Add(new SecurityRuleFault(SecurityMessage));
            return new SecurityRuleMatchResult(faults);
        }

        SecurityRuleMatchResult ISecurityRule.Match(object obj)
        {
            return Match((TProperty)obj);
        }

        public string SecurityMessage { get; protected set; }
    }
}
