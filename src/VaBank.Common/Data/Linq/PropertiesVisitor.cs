using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace VaBank.Common.Data.Linq
{
    internal class PropertiesVisitor : ExpressionVisitor
    {
        private readonly HashSet<string> _properties;

        private readonly Type _type;

        public static IEnumerable<string> GetUsedProperties<T>(Expression expression)
            where T : class
        {
            var visitor = new PropertiesVisitor(typeof(T));
            visitor.Visit(expression);
            return visitor._properties;
        }

        private PropertiesVisitor(Type type)
        {
            _properties = new HashSet<string>();
            _type = type;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var propertyPathVisitor = new PropertyPathVisitor();
            var member = node.Member;
            var path = member.DeclaringType != _type
                ? propertyPathVisitor.GetPropertyPath(node)
                : node.Member.Name;
            if (!_properties.Any(x => x.StartsWith(string.Format("{0}.", path))))
            {
                _properties.Add(path);
            }
            return base.VisitMember(node);
        }
    }
}
