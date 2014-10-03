using Newtonsoft.Json;
using VaBank.Common.Data.Sorting.Converters;

namespace VaBank.Common.Data.Sorting
{
    [JsonConverter(typeof (JsonSortDirectionConverter))]
    public enum SortDirection
    {
        Ascending,
        Descending
    }
}