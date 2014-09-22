using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Common.Filtration.Serialization
{
    public class JsonFilterDescriptorSerializer : IFilterSerializer<string>
    {
        public FilterDescriptor Deserialize(string json)
        {
            var jObj = JObject.Parse(json);
            var test = jObj.ToString();            
            switch (jObj.SelectToken("type").Value<string>().ToFilterType())
            {
                case FilterType.Combiner:
                    return new FilterDescriptor { Context = DeserializeCombineFilter(jObj) };
                case FilterType.Expression:
                    return new FilterDescriptor { Context = DeserializeExpressionFilter(jObj)};                
            }
            return null;
        }

        private CombinerFilter DeserializeCombineFilter(JToken jToken)
        {
            var filter = new CombinerFilter();
            filter.Logic = jToken.SelectToken("logic").Value<string>().ToFilterLogic();
            filter.Filters = new List<Filter>();
            foreach (var token in jToken.SelectToken("filters"))
            {
                switch (token.SelectToken("type").Value<string>().ToFilterType())
                {
                    case FilterType.Combiner:
                        filter.Filters.Add(DeserializeCombineFilter(token));
                        break;
                    case FilterType.Expression:
                        filter.Filters.Add(DeserializeExpressionFilter(token));
                        break;
                }
            }
            return filter;
        }

        private ExpressionFilter DeserializeExpressionFilter(JToken jToken)
        {
            return JsonConvert.DeserializeObject<ExpressionFilter>(jToken.ToString(), new FilterOperatorConverter());
        }

        public string Serialize(FilterDescriptor descriptor)
        {
            return JsonConvert.SerializeObject(descriptor.Context);
        }
    }
}
