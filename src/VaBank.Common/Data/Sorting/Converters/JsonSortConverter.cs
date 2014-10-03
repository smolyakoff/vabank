using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VaBank.Common.Serialization;

namespace VaBank.Common.Data.Sorting.Converters
{
    internal class JsonSortConverter : JsonCreationConverter<ISort>
    {
        public override bool CanWrite
        {
            get { return false; }
        }

        protected override ISort Create(Type objectType, JObject jObject, JsonSerializer serializer)
        {
            if (jObject["sorts"] != null)
            {
                return CallPrivateConstructor<MultiSort>();
            }
            return CallPrivateConstructor<SimpleSort>();
        }
    }
}