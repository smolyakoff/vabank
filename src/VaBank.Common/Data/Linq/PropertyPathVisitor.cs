using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace VaBank.Common.Data.Linq
{
    internal class PropertyPathVisitor : ExpressionVisitor
    {
        /// <summary>
        /// The stack to store the sequence of properties.
        /// </summary>
        private Stack<string> stack;

        /// <summary>
        /// Gets the property path.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>the property path as a string.</returns>
        public string GetPropertyPath(Expression expression)
        {
            this.stack = new Stack<string>();
            Visit(expression);
            return this.stack
            .Aggregate(
            new StringBuilder(),
            (sb, name) =>
            (sb.Length > 0 ? sb.Append(".") : sb).Append(name))
            .ToString();
        }

        /// <summary>
        /// Visits the children of the <see cref="MemberExpression"/>.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitMember(MemberExpression expression)
        {
            if (this.stack != null)
            {
                this.stack.Push(expression.Member.Name);
            }

            return base.VisitMember(expression);
        }

        /// <summary>
        /// Visits the children of the <see cref="MethodCallExpression"/>.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            if (IsLinqOperator(expression.Method))
            {
                for (int i = 1; i < expression.Arguments.Count; i++)
                {
                    Visit(expression.Arguments[i]);
                }

                Visit(expression.Arguments[0]);
                return expression;
            }

            return base.VisitMethodCall(expression);
        }

        /// <summary>
        /// Determines whether the specified method is a linq operator.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>
        /// <c>true</c> if it is a linq operator; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsLinqOperator(MethodInfo method)
        {
            if (method.DeclaringType != typeof(Queryable) && method.DeclaringType != typeof(Enumerable))
            {
                return false;
            }

            return Attribute.GetCustomAttribute(method, typeof(ExtensionAttribute)) != null;
        }
    }
}
