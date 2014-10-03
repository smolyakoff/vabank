using System;
using System.Linq.Expressions;
using VaBank.Common.Data.Linq;

namespace VaBank.Common.Data.Filtering
{
    public class SpecFilter<T> : LinqSpec<T>, IFilter<T>
        where T : class
    {
        private readonly Expression<Func<T, bool>> _expression;

        public SpecFilter(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            _expression = expression;
        }

        public override Expression<Func<T, bool>> Expression
        {
            get { return _expression; }
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            return _expression;
        }

        public Expression<Func<T1, bool>> ToExpression<T1>() where T1 : class
        {
            if (typeof (T1) != typeof (T))
            {
                throw new NotSupportedException("Expression enclosed type mismatch.");
            }
            return _expression as Expression<Func<T1, bool>>;
        }
    }
}