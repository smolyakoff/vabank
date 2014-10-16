using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VaBank.Common.Events
{
    public class EventJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null; 
            }
            var jObject = JToken.ReadFrom(reader);
            var type = jObject["$eventType"].ToObject<Type>();
            return serializer.Deserialize(reader, type);
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (Event).IsAssignableFrom(objectType);
        }
    }
}
