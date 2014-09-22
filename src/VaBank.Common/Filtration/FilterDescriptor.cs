using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Common.Filtration
{
    public class FilterDescriptor
    {        
        public Filter Context { get; set; }

        public Expression<Func<T, bool>> ToExpression<T>()
        {
            throw new NotImplementedException();
        }
    }
}
