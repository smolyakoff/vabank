using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaBank.Common.Data.Filtering
{
    [JsonConverter(typeof (StringEnumConverter))]
    public enum FilterLogic
    {
        And,
        Or
    }
}