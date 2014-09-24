using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Common.Sorting.Serialization
{
    public class SortingDescriptorSerializer : IJsonSortingDescriptorSerializer
    {
        public string Serialize(SortingDescriptor descriptor)
        {
            return JsonConvert.SerializeObject(descriptor);
        }

        public SortingDescriptor Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<SortingDescriptor>(json);
        }
    }
}
