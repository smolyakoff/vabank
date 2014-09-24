using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Common.Sorting
{
    [Serializable]
    public class SortingDescriptor
    {
        public ICollection<Sort> Sortings { get; set; }
    }
}
