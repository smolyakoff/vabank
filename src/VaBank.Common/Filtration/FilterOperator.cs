using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaBank.Common.Filtration
{
    public enum FilterOperator
    {
        Equality,
        Inequality,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        In,
        NotIn,
        StartsWith,
        NotStartsWith,
        EndsWith,
        NotEndsWith,
        Contains,
        NotContains
    }
}
