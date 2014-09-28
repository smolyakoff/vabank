using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Common.Sorting.Serialization
{
    public class JsonSortingDescriptorSerializer : IJsonSortingDescriptorSerializer
    {
        public string Serialize(SortingDescriptor descriptor)
        {
            return JsonConvert.SerializeObject(descriptor.Sortings);
        }

        public SortingDescriptor Deserialize(string json)
        {
            return new SortingDescriptor { Sortings = JsonConvert.DeserializeObject<ICollection<Sort>>(json) };
        }
    }
}
