using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Common.Filtration
{
    [Serializable]
    public class FilterDescriptor
    {
        public Filter Context { get; set; }        
    }
}
