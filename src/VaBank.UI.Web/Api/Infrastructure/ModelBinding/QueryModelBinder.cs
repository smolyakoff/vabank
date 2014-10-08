using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VaBank.Common.Data;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Filtering.Converters;
using VaBank.Common.Data.Paging;
using VaBank.Common.Data.Sorting;
using VaBank.Common.Data.Sorting.Converters;

namespace VaBank.UI.Web.Api.Infrastructure.ModelBinding
{
    public class QueryModelBinder : IModelBinder
    {
        private static class Keys
        {
            public const string Filter = "filter";

            public const string Sort = "sort";

            public const string PageSize = "pageSize";

            public const string PageNumber = "pageNumber";

        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (!typeof (IClientQuery).IsAssignableFrom(bindingContext.ModelType))
            {
                return false;
            }
            bindingContext.Model = actionContext.Request.Method == HttpMethod.Get || actionContext.Request.Method == HttpMethod.Delete
                ? BindFromQueryStringAndRouteData(actionContext, bindingContext) 
                : BindFromBody(actionContext, bindingContext);
            return true;
        }

        private IClientQuery BindFromBody(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var memoryStream = new MemoryStream();
            actionContext.Request.Content.ReadAsStreamAsync().Result.CopyTo(memoryStream);
            memoryStream.Position = 0;
            var content = new StreamContent(memoryStream);
            actionContext.Request.Content.Headers.AsEnumerable().ToList().ForEach(x => content.Headers.TryAddWithoutValidation(x.Key, x.Value));
            var query = content.ReadAsAsync(bindingContext.ModelType, actionContext.ControllerContext.Configuration.Formatters).Result as IClientQuery;
            if (query == null)
            {
                return null;
            }
            var clientFilterable = query as IClientFilterable;
            var clientSortable = query as IClientSortable;
            var clientPageable = query as IClientPageable;
            memoryStream.Position = 0;
            var apiQuery = content.ReadAsAsync(typeof(ApiQuery), actionContext.ControllerContext.Configuration.Formatters).Result as ApiQuery;
            if (apiQuery == null)
            {
                return query;
            }
            if (clientFilterable != null && apiQuery.Filter != null && !(apiQuery.Filter is AlwaysTrueFilter))
            {
                clientFilterable.ClientFilter = apiQuery.Filter;
            }
            if (clientSortable != null && apiQuery.Sort != null && !(apiQuery.Sort is RandomSort))
            {
                clientSortable.ClientSort = apiQuery.Sort;
            }
            if (clientPageable != null && (apiQuery.PageSize != null && apiQuery.PageNumber != null))
            {
                clientPageable.ClientPage = new ClientPage
                {
                    PageNumber = apiQuery.PageNumber,
                    PageSize = apiQuery.PageSize
                };
            }
            return query;
        }

        private IClientQuery BindFromQueryStringAndRouteData(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var query = Activator.CreateInstance(bindingContext.ModelType) as IClientQuery;
            var clientFilterable = query as IClientFilterable;
            var clientSortable = query as IClientSortable;
            var clientPageable = query as IClientPageable;
            var queryString = actionContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => (object) x.Value);
            var routeDataValues = actionContext.Request.GetRouteData().Values;
            var requestValues = queryString.Union(routeDataValues).ToDictionary(x => x.Key, x => x.Value);
            if (queryString.ContainsKey(Keys.Filter) && clientFilterable != null)
            {
                var converter = new QueryStringFilterConverter();
                var filter = (IFilter) converter.ConvertFrom(queryString[Keys.Filter]);
                clientFilterable.ClientFilter = filter ?? new AlwaysTrueFilter();
            }
            if (queryString.ContainsKey(Keys.Sort) && clientSortable != null)
            {
                var converter = new SortTypeConverter();
                var sort = (ISort)converter.ConvertFrom(queryString[Keys.Sort]);
                clientSortable.ClientSort = sort ?? new RandomSort();
            }
            if ((queryString.ContainsKey(Keys.PageNumber) || queryString.ContainsKey(Keys.PageSize)) && clientPageable != null)
            {
                int pageNumber, pageSize;
                var hasNumber = int.TryParse(requestValues[Keys.PageNumber].ToString(), out pageNumber);
                var hasSize = int.TryParse(requestValues[Keys.PageSize].ToString(), out pageSize);
                var page = new ClientPage
                {
                    PageNumber = hasNumber ? (int?) pageNumber : null,
                    PageSize = hasSize ? (int?) pageSize : null
                };
                clientPageable.ClientPage = page;
            }
            //Populate other properties here
            var knownKeys = new List<string> {Keys.Filter, Keys.Sort, Keys.PageNumber, Keys.PageSize};
            var otherProperties = requestValues
                .Where(x => !knownKeys.Contains(x.Key))
                .ToDictionary(x => x.Key, x => x.Value);
                
            var otherJson = JsonConvert.SerializeObject(otherProperties);
            JsonConvert.PopulateObject(otherJson, query, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return query;
        }

        private class ApiQuery
        {
             public IFilter Filter { get; set; }

             public ISort Sort { get; set; }

             public int? PageNumber { get; set; }

             public int? PageSize { get; set; }
        }
    }
}