using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace VaBank.Common.Data.Filtering.Converters
{
    internal class JsonFilterPropertyTypeConverter : JsonConverter
    {
        private static readonly Dictionary<string, FilterPropertyType> TypeMapping;

        private static readonly Dictionary<FilterPropertyType, string> TypeReverseMapping;

        static JsonFilterPropertyTypeConverter()
        {
            TypeMapping = new Dictionary<string, FilterPropertyType>(StringComparer.OrdinalIgnoreCase)
            {
                { "auto", FilterPropertyType.Auto },
                { "byte", FilterPropertyType.Byte },
                { "short", FilterPropertyType.Short },
                { "int", FilterPropertyType.Int },
                { "long", FilterPropertyType.Long},
                { "char", FilterPropertyType.Char },
                { "string", FilterPropertyType.String },
                { "float", FilterPropertyType.Float },
                { "double", FilterPropertyType.Double },
                { "decimal", FilterPropertyType.Decimal },
                { "datetime", FilterPropertyType.DateTime },
                { "guid", FilterPropertyType.Guid },
                { "boolean", FilterPropertyType.Boolean },
            };
            TypeReverseMapping = TypeMapping.ToDictionary(x => x.Value, x => x.Key);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!TypeReverseMapping.ContainsKey((FilterPropertyType)value))
            {
                throw new NotSupportedException(string.Format("Operator [{0}] is not supported.", value));
            }
            writer.WriteValue(TypeReverseMapping[(FilterPropertyType)value]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value as string;
            if (string.IsNullOrEmpty(value) || !TypeMapping.ContainsKey(value))
            {
                throw new NotSupportedException(string.Format("Operator [{0}] is not supported.", value));
            }
            return TypeMapping[value];
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FilterPropertyType);
        }
    }
}
