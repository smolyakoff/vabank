using System;
using Newtonsoft.Json;

namespace VaBank.Common.Resources
{
    public class LinkJsonConverter : JsonConverter
    {
        private readonly IUriProvider _uriProvider;

        public LinkJsonConverter(IUriProvider uriProvider)
        {
            if (uriProvider == null)
            {
                throw new ArgumentNullException("uriProvider");
            }
            _uriProvider = uriProvider;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var link = value as Link;
            if (link == null)
            {
                writer.WriteNull();
                return;
            }
            var uri = _uriProvider.GetUri(link.Uri, link.Location);
            writer.WriteValue(uri);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (Link).IsAssignableFrom(objectType);
        }
    }
}
