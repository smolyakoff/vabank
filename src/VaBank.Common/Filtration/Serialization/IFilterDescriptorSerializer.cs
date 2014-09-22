using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Common.Filtration.Serialization
{
    public interface IFilterSerializer<T>
    {
        T Serialize(FilterDescriptor descriptor);
        FilterDescriptor Deserialize(T source);
    }
}
