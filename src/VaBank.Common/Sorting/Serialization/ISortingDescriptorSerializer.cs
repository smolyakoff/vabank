using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Common.Sorting.Serialization
{
    public interface ISortingDescriptorSerializer<T>
    {
        T Serialize(SortingDescriptor descriptor);
        SortingDescriptor Deserialize(T source);
    }
}
