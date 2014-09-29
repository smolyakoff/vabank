using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Common.Data.Filtering;

namespace VaBank.Common.Filtration
{
    [Serializable]
    public class CombinerFilter : Filter
    {
        public FilterLogic Logic { get; set; }
        public ICollection<Filter> Filters { get; set; }

        public CombinerFilter()
        {
            Type = FilterType.Combiner;
        }
    }
}
