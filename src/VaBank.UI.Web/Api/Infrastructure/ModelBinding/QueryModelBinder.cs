using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using VaBank.Common.Data;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Filtering.Converters;

namespace VaBank.UI.Web.Api.Infrastructure.ModelBinding
{
    public class QueryModelBinder : IModelBinder
    {
        private static class Keys
        {
            public const string Filter = "filter";
           
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (!typeof (IQuery).IsAssignableFrom(bindingContext.ModelType))
            {
                return false;
            }
            bindingContext.Model = actionContext.Request.Method == HttpMethod.Get || actionContext.Request.Method == HttpMethod.Delete
                ? BindFromQueryString(actionContext, bindingContext) 
                : BindFromBody(actionContext, bindingContext);
            return true;
        }

        private IQuery BindFromBody(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var memoryStream = new MemoryStream();
            actionContext.Request.Content.ReadAsStreamAsync().Result.CopyTo(memoryStream);
            memoryStream.Position = 0;
            var content = new StreamContent(memoryStream);
            actionContext.Request.Content.Headers.AsEnumerable().ToList().ForEach(x => content.Headers.TryAddWithoutValidation(x.Key, x.Value));
            var query = content.ReadAsAsync(bindingContext.ModelType, actionContext.ControllerContext.Configuration.Formatters).Result as IQuery;
            if (query == null)
            {
                return null;
            }
            var clientFilterable = query as IClientFilterableQuery;
            memoryStream.Position = 0;
            var apiQuery = content.ReadAsAsync(typeof(ApiQuery), actionContext.ControllerContext.Configuration.Formatters).Result as ApiQuery;
            if (apiQuery == null)
            {
                return query;
            }
            if (apiQuery.Filter != null && !apiQuery.Filter.IsEmpty() && clientFilterable != null)
            {
                clientFilterable.ApplyFilter(apiQuery.Filter);
            }
            return query;
        }

        private IQuery BindFromQueryString(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var query = Activator.CreateInstance(bindingContext.ModelType) as IQuery;
            var clientFilterable = query as IClientFilterableQuery;
            var queryString = actionContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
            if (queryString.ContainsKey(Keys.Filter) && clientFilterable != null)
            {
                var converter = new QueryStringFilterConverter();
                var filter = (Common.Data.Filtering.IFilter) converter.ConvertFrom(queryString[Keys.Filter]);
                clientFilterable.ApplyFilter(filter);
            }
            //Populate other properties here
            var knownKeys = new List<string> {Keys.Filter};
            var otherProperties = queryString.Where(x => !knownKeys.Contains(x.Key))
                .ToDictionary(x => x.Key, x => x.Value);
            var otherJson = JsonConvert.SerializeObject(otherProperties);
            JsonConvert.PopulateObject(otherJson, query);
            return query;
        }

        private class ApiQuery
        {
             public IFilter Filter { get; set; }
        }
    }
}