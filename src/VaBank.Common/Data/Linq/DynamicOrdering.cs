using System.Linq.Expressions;

namespace VaBank.Common.Data.Linq
{
    internal class DynamicOrdering
    {
        public Expression Selector;
        public bool Ascending;
    }
}
