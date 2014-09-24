using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaBank.Common.Sorting
{
    [Serializable]
    public class Sort
    {
        public string Property { get; set; }
        public SortingDirection Direction { get; set; }
    }
}
