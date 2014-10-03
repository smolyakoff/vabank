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
            if (!typeof (IQuery).IsAssignableFrom(bindingContext.ModelType))
            {
                return false;
            }
            bindingContext.Model = actionContext.Request.Method == HttpMethod.Get || actionContext.Request.Method == HttpMethod.Delete
                ? BindFromQueryStringAndRouteData(actionContext, bindingContext) 
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
                clientFilterable.ApplyFilter(apiQuery.Filter);
            }
            if (clientSortable != null && apiQuery.Sort != null && !(apiQuery.Sort is RandomSort))
            {
                clientSortable.ApplySort(apiQuery.Sort);
            }
            if (clientPageable != null && (apiQuery.PageSize != null && apiQuery.PageNumber != null))
            {
                clientPageable.ApplyPaging(apiQuery.PageNumber, apiQuery.PageSize);
            }
            return query;
        }

        private IQuery BindFromQueryStringAndRouteData(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var query = Activator.CreateInstance(bindingContext.ModelType) as IQuery;
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
                clientFilterable.ApplyFilter(filter);
            }
            if (queryString.ContainsKey(Keys.Sort) && clientSortable != null)
            {
                var converter = new SortTypeConverter();
                var sort = (ISort)converter.ConvertFrom(queryString[Keys.Sort]);
                clientSortable.ApplySort(sort);
            }
            if ((queryString.ContainsKey(Keys.PageNumber) || queryString.ContainsKey(Keys.PageSize)) && clientPageable != null)
            {
                int pageNumber, pageSize;
                var hasNumber = int.TryParse(requestValues[Keys.PageNumber].ToString(), out pageNumber);
                var hasSize = int.TryParse(requestValues[Keys.PageSize].ToString(), out pageSize);
                clientPageable.ApplyPaging(hasNumber ? (int?)pageNumber : null, hasSize ? (int?)pageSize : null);
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