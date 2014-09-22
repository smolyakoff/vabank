using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaBank.Common.Filtration
{
    public abstract class Filter
    {
        [JsonIgnore]
        public FilterType Type { get; protected set; }        
    }
}
