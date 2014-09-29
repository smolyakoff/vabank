using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VaBank.Common.Serialization
{
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            // Load JObject from stream
            var jObject = JObject.Load(reader);

            // Create target object based on JObject
            var target = Create(objectType, jObject);

            //Create a new reader for this jObject, and set all properties to match the original reader.
            JsonReader jObjectReader = jObject.CreateReader();
            jObjectReader.Culture = reader.Culture;
            jObjectReader.DateParseHandling = reader.DateParseHandling;
            jObjectReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            jObjectReader.FloatParseHandling = reader.FloatParseHandling;

            // Populate the object properties
            serializer.Populate(jObjectReader, target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        protected static T1 CallPrivateConstructor<T1>()
        {
            var constructor = typeof(T1).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            return (T1)constructor.Invoke(null);
        }
    }
}
