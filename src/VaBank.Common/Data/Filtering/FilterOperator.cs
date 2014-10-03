using Newtonsoft.Json;
using VaBank.Common.Data.Filtering.Converters;

namespace VaBank.Common.Data.Filtering
{
    [JsonConverter(typeof (JsonFilterOperatorConverter))]
    public enum FilterOperator
    {
        Equal,
        NotEqual,
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