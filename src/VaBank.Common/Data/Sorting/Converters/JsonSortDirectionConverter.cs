using System;
using Newtonsoft.Json;

namespace VaBank.Common.Data.Sorting.Converters
{
    internal class JsonSortDirectionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var sortDirection = (SortDirection) value;
            writer.WriteValue(sortDirection.ToSqlString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var stringSort = reader.Value as string;
            return SortingExtensions.ToSortDirection(stringSort);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (SortDirection) == objectType;
        }
    }
}