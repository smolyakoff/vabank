using Newtonsoft.Json;
using VaBank.Common.Data.Filtering.Converters;

namespace VaBank.Common.Data.Filtering
{
    [JsonConverter(typeof (JsonFilterPropertyTypeConverter))]
    public enum FilterPropertyType
    {
        Auto,
        Byte,
        Short,
        Int,
        Long,
        Char,
        String,
        Float,
        Double,
        Decimal,
        DateTime,
        Guid,
        Boolean
    }
}