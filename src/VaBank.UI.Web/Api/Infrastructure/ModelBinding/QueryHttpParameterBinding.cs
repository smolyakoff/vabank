using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using VaBank.Common.Data;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Filtering.Converters;
using VaBank.Common.Data.Paging;
using VaBank.Common.Data.Sorting;
using VaBank.Common.Data.Sorting.Converters;

namespace VaBank.UI.Web.Api.Infrastructure.ModelBinding
{
    public class QueryHttpParameterBinding : HttpParameterBinding
    {
        private readonly HttpParameterDescriptor _descriptor;

        private static class Keys
        {
            public const string Filter = "filter";

            public const string Sort = "sort";

            public const string PageSize = "pageSize";

            public const string PageNumber = "pageNumber";

            public static readonly List<string> KnownKeys = new List<string>()
            {
                Filter,
                PageNumber,
                PageSize,
                Sort
            };

        }

        public QueryHttpParameterBinding(HttpParameterDescriptor descriptor) : base(descriptor)
        {
            _descriptor = descriptor;
        }

        public override async Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider, HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            if (actionContext.Request.Method == HttpMethod.Get || actionContext.Request.Method == HttpMethod.Delete)
            {
                await BindFromUri(metadataProvider, actionContext, cancellationToken);
            }
            else
            {
                await BindFromBody(metadataProvider, actionContext, cancellationToken);
            }
        }

        private async Task BindFromBody(ModelMetadataProvider metadataProvider, HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var bodyBinding = _descriptor.BindWithAttribute(new FromBodyAttribute());
            var memoryStream = new MemoryStream();
            await actionContext.Request.Content.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var content = new StreamContent(memoryStream);
            actionContext.Request.Content.Headers.AsEnumerable().ToList().ForEach(x => content.Headers.TryAddWithoutValidation(x.Key, x.Value));
            actionContext.Request.Content = new StreamContent(memoryStream);
            await bodyBinding.ExecuteBindingAsync(metadataProvider, actionContext, cancellationToken);
            var query = GetValue(actionContext) as IClientQuery;
            if (query == null)
            {
                SetValue(actionContext, null);
                return;
            }
            var clientFilterable = query as IClientFilterable;
            var clientSortable = query as IClientSortable;
            var clientPageable = query as IClientPageable;
            memoryStream.Position = 0;
            var apiQuery = await content.ReadAsAsync(typeof(ApiQuery), actionContext.ControllerContext.Configuration.Formatters, cancellationToken) as ApiQuery;
            if (apiQuery == null)
            {
                return;
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
            SetValue(actionContext, query);
        }

        private async Task BindFromUri(ModelMetadataProvider metadataProvider, HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            var uriBinding = _descriptor.BindWithAttribute(new FromUriAttribute());
            await uriBinding.ExecuteBindingAsync(metadataProvider, actionContext, cancellationToken);
            var query = GetValue(actionContext) as IClientQuery ??
                        Activator.CreateInstance(_descriptor.ParameterType) as IClientQuery;
            var clientFilterable = query as IClientFilterable;
            var clientSortable = query as IClientSortable;
            var clientPageable = query as IClientPageable;
            var queryString = actionContext.Request.GetQueryNameValuePairs()
                .Where(x => Keys.KnownKeys.Contains(x.Key))
                .ToDictionary(x => x.Key, x => (object)x.Value);
            var routeDataValues = actionContext.Request.GetRouteData()
                .Values
                .Where(x => Keys.KnownKeys.Contains(x.Key));
            var requestValues = queryString.Union(routeDataValues).ToDictionary(x => x.Key, x => x.Value);
            if (queryString.ContainsKey(Keys.Filter) && clientFilterable != null)
            {
                var converter = new QueryStringFilterConverter();
                var filter = (IFilter)converter.ConvertFrom(queryString[Keys.Filter]);
                clientFilterable.ClientFilter = filter ?? new AlwaysTrueFilter();
            }
            if (queryString.ContainsKey(Keys.Sort) && clientSortable != null)
            {
                var converter = new SortTypeConverter();
                var sort = (ISort)converter.ConvertFrom(queryString[Keys.Sort]);
                clientSortable.ClientSort = sort ?? clientSortable.ClientSort ?? new RandomSort();
            }
            if ((queryString.ContainsKey(Keys.PageNumber) || queryString.ContainsKey(Keys.PageSize)) && clientPageable != null)
            {
                int pageNumber = 1, pageSize = 10;
                var hasNumber = requestValues.ContainsKey(Keys.PageNumber) && int.TryParse(requestValues[Keys.PageNumber].ToString(), out pageNumber);
                var hasSize = requestValues.ContainsKey(Keys.PageSize) && int.TryParse(requestValues[Keys.PageSize].ToString(), out pageSize);
                var page = new ClientPage
                {
                    PageNumber = hasNumber ? (int?)pageNumber : null,
                    PageSize = hasSize ? (int?)pageSize : null
                };
                clientPageable.ClientPage = page;
            }
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