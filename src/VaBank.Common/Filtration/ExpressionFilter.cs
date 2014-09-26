using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Common.Filtration
{
    [Serializable]
    public class ExpressionFilter : Filter
    {
        public string Property { get; set; }
        public FilterOperator Operator { get; set; }
        public object Value { get; set; }

        public ExpressionFilter()
        {
            Type = FilterType.Expression;
        }
    }
}
