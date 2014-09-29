using Newtonsoft.Json;
using VaBank.Common.Filtration.Serialization;

namespace VaBank.Common.Data.Filtering
{
    [JsonConverter(typeof(FilterOperatorConverter))]
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
