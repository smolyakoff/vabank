using System;
using System.Linq.Expressions;

namespace VaBank.Services.Contracts.Common.Models
{
    public class UserMessage
    {
        public static UserMessage Format(string format, params object[] parameters)
        {
            return new UserMessage(string.Format(format, parameters));
        }

        public static UserMessage Format(string format, string code, params object[] parameters)
        {
            return new UserMessage(string.Format(format, parameters), code);
        }

        public static UserMessage Resource(Expression<Func<string>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("Member expression is required.", "expression");
            }
            var getter = expression.Compile();
            return new UserMessage(getter(), memberExpression.Member.Name);
        }

        public static UserMessage ResourceFormat(Expression<Func<string>> expression, params object[] parameters)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("Member expression is required.", "expression");
            }
            var getter = expression.Compile();
            var message = string.Format(getter(), parameters);
            return new UserMessage(message, memberExpression.Member.Name);
        }

        public UserMessage(string message, string code = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }
            Message = message;
            Code = code;
        }

        public string Message { get; private set; }

        public string Code { get; set; }
    }
}
