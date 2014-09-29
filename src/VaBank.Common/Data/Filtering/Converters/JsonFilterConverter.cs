using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using VaBank.Common.Serialization;

namespace VaBank.Common.Data.Filtering.Converters
{
    internal class JsonFilterConverter : JsonCreationConverter<IFilter>
    {
        private const string TypeFieldName = "type";

        private static readonly Dictionary<string, Func<IFilter>>  _constructors = 
            new Dictionary<string, Func<IFilter>>(StringComparer.OrdinalIgnoreCase);

        static JsonFilterConverter()
        {
            _constructors.Add("simple", CallPrivateConstructor<SimpleFilter>);
            _constructors.Add("combined", () => new CombinedFilter());
            _constructors.Add("linq", CallPrivateConstructor<DynamicLinqFilter>);
        }

        protected override IFilter Create(Type objectType, JObject jObject)
        {
            var typeToken = jObject[TypeFieldName];
            var type = typeToken == null ? null : typeToken.Value<string>();
            if (typeToken == null || string.IsNullOrEmpty(type))
            {
                return new EmptyFilter();
            }
            return !_constructors.ContainsKey(type) ? new EmptyFilter() : _constructors[type]();
        }
    }
}
