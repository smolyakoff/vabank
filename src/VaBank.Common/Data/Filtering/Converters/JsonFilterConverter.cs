using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VaBank.Common.Serialization;

namespace VaBank.Common.Data.Filtering.Converters
{
    internal class JsonFilterConverter : JsonCreationConverter<IFilter>
    {
        private const string TypeFieldName = "type";

        private const string ValueFieldName = "value";

        private const string PropertyTypeFieldName = "propertyType";

        private static readonly Dictionary<string, Func<IFilter>>  _constructors = 
            new Dictionary<string, Func<IFilter>>(StringComparer.OrdinalIgnoreCase);


        static JsonFilterConverter()
        {
            _constructors.Add("simple", CallPrivateConstructor<SimpleFilter>);
            _constructors.Add("combined", () => new CombinedFilter());
            _constructors.Add("linq", CallPrivateConstructor<DynamicLinqFilter>);
        }

        protected override IFilter Create(Type objectType, JObject jObject, JsonSerializer serializer)
        {
            var typeToken = jObject[TypeFieldName];
            var type = typeToken == null ? null : typeToken.Value<string>();
            if (typeToken == null || string.IsNullOrEmpty(type))
            {
                return new EmptyFilter();
            }
            if (type == "simple")
            {
                var propertyTypeToken = jObject[PropertyTypeFieldName];
                var propertyType = propertyTypeToken == null
                    ? FilterPropertyType.Auto
                    : serializer.Deserialize<FilterPropertyType>(propertyTypeToken.CreateReader());
                var valueToken = jObject[ValueFieldName];
                if (valueToken != null)
                {
                    BindValueToken(valueToken, propertyType, serializer);
                }
            }
            return !_constructors.ContainsKey(type) ? new EmptyFilter() : _constructors[type]();
        }

        private void BindValueToken(JToken token, FilterPropertyType propertyType, JsonSerializer serializer)
        {
            if (token is JValue)
            {
                BindValue((JValue)token, propertyType, serializer);
            } else if (token is JArray)
            {
                BindArray((JArray) token, propertyType, serializer);
            }
        }

        private void BindArray(JArray token, FilterPropertyType propertyType, JsonSerializer serializer)
        {
            token.Cast<JValue>().ToList().ForEach(x => BindValue(x, propertyType, serializer));
        }

        private void BindValue(JValue jValue, FilterPropertyType propertyType, JsonSerializer serializer)
        {
            var type = propertyType.ToType();
            if (jValue.Value.GetType() != type)
            {
                jValue.Value = serializer.Deserialize(jValue.CreateReader(), type);
            }
        }

    }
}
