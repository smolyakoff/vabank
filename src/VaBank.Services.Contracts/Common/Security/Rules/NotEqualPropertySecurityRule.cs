using System;
using System.Linq.Expressions;

namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public class NotEqualPropertySecurityRule<TProperty> : ConstraintPropertySecurityRule<TProperty>
    {
        private const string DefaultMessage = "NotEqualPropertyRule security error. Constraint: {0}.";

        public NotEqualPropertySecurityRule(string message, TProperty value) 
            : base(GetConstraint(value), message)
        {
        }

        public NotEqualPropertySecurityRule(TProperty value)
            : base(GetConstraint(value), string.Format(DefaultMessage, GetConstraint(value).ToString()))
        {
        }

        private static Expression<Func<TProperty, bool>> GetConstraint(TProperty value)
        {
            return x => !x.Equals(value);
        }
    }
}
