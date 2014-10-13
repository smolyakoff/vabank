using System;
using System.Linq.Expressions;

namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public class EqualPropertySecurityRule<TProperty> : ConstraintPropertySecurityRule<TProperty>
    {
        private const string DefaultMessage = "EqualPropertyRule security error. Constraint: {0}.";

        public EqualPropertySecurityRule(string message, TProperty value)
            : base(GetConstraint(value), message)
        {
        }

        public EqualPropertySecurityRule(TProperty value)
            : base(GetConstraint(value), string.Format(DefaultMessage, GetConstraint(value).ToString()))
        {
        }

        private static Expression<Func<TProperty, bool>> GetConstraint(TProperty value)
        {
            return x => x.Equals(value);
        }
    }
}
