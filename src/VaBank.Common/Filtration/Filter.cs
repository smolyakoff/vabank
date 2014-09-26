using System.ComponentModel;
using Newtonsoft.Json;
using VaBank.Common.Filtration.Serialization;

namespace VaBank.Common.Filtration
{
    [TypeConverter(typeof(FilterTypeConverter))]
    public abstract class Filter
    {
        [JsonIgnore]
        public FilterType Type { get; protected set; }        
    }
}
