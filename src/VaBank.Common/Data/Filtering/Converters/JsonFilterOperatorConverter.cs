using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace VaBank.Common.Data.Filtering.Converters
{
    internal class JsonFilterOperatorConverter : JsonConverter
    {
        private static readonly Dictionary<string, FilterOperator> FilterMapping;

        private static readonly Dictionary<FilterOperator, string> FilterReverseMapping;

        static JsonFilterOperatorConverter()
        {
            FilterMapping = new Dictionary<string, FilterOperator>(StringComparer.OrdinalIgnoreCase)
            {
                { "==", FilterOperator.Equal },
                { "!=", FilterOperator.NotEqual },
                { ">", FilterOperator.GreaterThan },
                { "<", FilterOperator.LessThan },
                { ">=", FilterOperator.GreaterThanOrEqual },
                { "<=", FilterOperator.LessThanOrEqual },
                { "in", FilterOperator.In },
                { "!in", FilterOperator.NotIn },
                { "startswith", FilterOperator.StartsWith },
                { "!startswith", FilterOperator.NotStartsWith },
                { "endswith", FilterOperator.EndsWith },
                { "!endswith", FilterOperator.NotEndsWith },
                { "contains", FilterOperator.Contains },
                { "!contains", FilterOperator.NotContains },
            };
            FilterReverseMapping = FilterMapping.ToDictionary(x => x.Value, x => x.Key);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!FilterReverseMapping.ContainsKey((FilterOperator)value))
            {
                throw new NotSupportedException(string.Format("Operator [{0}] is not supported.", value));
            }
            writer.WriteValue(FilterReverseMapping[(FilterOperator)value]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value as string;
            if (string.IsNullOrEmpty(value) || !FilterMapping.ContainsKey(value))
            {
                throw new NotSupportedException(string.Format("Operator [{0}] is not supported.", value));
            }
            return FilterMapping[value];
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (FilterOperator);
        }
    }
}
