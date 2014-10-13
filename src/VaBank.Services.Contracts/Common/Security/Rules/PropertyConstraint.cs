using System;
using System.Linq.Expressions;

namespace VaBank.Services.Contracts.Common.Security.Rules
{
    public class PropertyConstraint<TProperty>
    {
        public PropertyConstraint(string message, Expression<Func<TProperty, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException("message");
            Message = message;
            Predicate = predicate;
        }

        public string Message { get; private set; }

        public Expression<Func<TProperty, bool>> Predicate { get; private set; }
    }
}
