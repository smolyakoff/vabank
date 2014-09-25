using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VaBank.Common.Filtration.Serialization;

namespace VaBank.Common.Filtration
{
    public abstract class Filter
    {
        [JsonIgnore]
        public FilterType Type { get; protected set; }        
    }
}
