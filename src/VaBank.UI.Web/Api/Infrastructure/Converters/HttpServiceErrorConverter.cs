using System;
using Newtonsoft.Json;
using VaBank.UI.Web.Api.Infrastructure.Models;

namespace VaBank.UI.Web.Api.Infrastructure.Converters
{
    public class HttpServiceErrorConverter : JsonConverter
    {
        public override bool CanRead
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var error = value as HttpServiceError;
            serializer.Serialize(writer, error == null ? null : error.HttpError);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (HttpServiceError).IsAssignableFrom(objectType);
        }
    }
}