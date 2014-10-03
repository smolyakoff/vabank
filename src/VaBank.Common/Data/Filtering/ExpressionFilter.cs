using System;
using System.Linq.Expressions;

namespace VaBank.Common.Data.Filtering
{
    public class ExpressionFilter<T> : IFilter<T>
        where T : class
    {
        private readonly Expression<Func<T, bool>> _expression;

        public ExpressionFilter(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            _expression = expression;
        }

        public virtual Expression<Func<T, bool>> ToExpression()
        {
            return ToExpression<T>();
        }

        public virtual Expression<Func<T1, bool>> ToExpression<T1>()
            where T1 : class
        {
            if (typeof (T1) != typeof (T))
            {
                throw new NotSupportedException("Expression enclosed type mismatch.");
            }
            return _expression as Expression<Func<T1, bool>>;
        }

        public static implicit operator ExpressionFilter<T>(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            return new ExpressionFilter<T>(expression);
        }

        public static implicit operator Expression<Func<T, bool>>(ExpressionFilter<T> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            return filter._expression;
        }
    }
}