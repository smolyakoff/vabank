using System;
using Newtonsoft.Json;
using VaBank.Services.Contracts.Common.Validation;

namespace VaBank.UI.Web.Api.Infrastructure.Converters
{
    public class ValidationExceptionSerializer : JsonConverter
    {
        public override bool CanRead
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var validationException = value as ValidationException;
            if (validationException == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var error = new
            {
                type = "validation",
                message = validationException.Message,
                faults = validationException.Faults
            };
            serializer.Serialize(writer, error);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (ValidationException).IsAssignableFrom(objectType);
        }
    }
}