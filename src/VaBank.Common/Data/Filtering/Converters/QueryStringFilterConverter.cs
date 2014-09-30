using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VaBank.Common.Data.Filtering.Converters
{
    public class QueryStringFilterConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof (string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            try
            {
                var stringValue = value as string;
                if (string.IsNullOrEmpty(stringValue))
                {
                    return new EmptyFilter();
                }
                var filterQuery = JsonConvert.DeserializeObject<QueryStringFilter>(stringValue);
                var filter = new DynamicLinqFilter(filterQuery.Query, filterQuery.Parameters);
                return filter;
            }
            catch (Exception)
            {
                return new EmptyFilter();
            }
        }

        private class QueryStringFilter
        {
            [JsonProperty("q")]
            public string Query { get; set; }

            public object[] Parameters
            {
                get { return GetParameters(); }
            }

            private object[] GetParameters()
            {
                var types = TypesCsv.Split(',')
                    .Select(x => x.Trim())
                    .Select(x => string.Format("\"{0}\"", x))
                    .Select(JsonConvert.DeserializeObject<FilterPropertyType>)
                    .Select(x => x.ToType())
                    .ToList();
                var parameters = JsonConvert.DeserializeObject<object[]>(ParametersJson);
                return parameters.Select((o, i) => Visit((dynamic) o, types[i])).Cast<object>().ToArray();
            }

            private object Visit(JArray jArray, Type type)
            {
                if (jArray == null)
                {
                    return null;
                }
                var listType = typeof (List<>).MakeGenericType(type);
                if (jArray.Count == 0)
                {
                    return Activator.CreateInstance(listType);
                }
                if (type == typeof (object))
                {
                    return InferType(jArray);
                }
                return jArray.ToObject(listType);
            }

            private object InferType(JArray array)
            {
                var firstTypedValue = array.Cast<JValue>().FirstOrDefault(x => x.Value != null);
                if (firstTypedValue == null)
                {
                    return new List<object>();
                }
                var type = firstTypedValue.Value.GetType();
                var listType = typeof(List<>).MakeGenericType(type);
                return array.ToObject(listType);
            }

            private object Visit(object obj, Type type)
            {
                return Convert.ChangeType(obj, type);
            }

            [JsonProperty("p", Required = Required.Always)]
            private string ParametersJson { get; set; }

            [JsonProperty("t", Required = Required.Always)]
            private string TypesCsv { get; set; }
        }
    }
}
