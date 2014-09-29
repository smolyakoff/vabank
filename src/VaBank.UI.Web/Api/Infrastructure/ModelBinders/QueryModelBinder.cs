using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using VaBank.Common.Data;
using VaBank.Common.Data.Filtering;
using VaBank.Common.Data.Filtering.Converters;

namespace VaBank.UI.Web.Api.Infrastructure.ModelBinders
{
    public class QueryModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            bindingContext.Model = Activator.CreateInstance(bindingContext.ModelType);
            var filterable = typeof (IFilterableQuery).IsAssignableFrom(bindingContext.ModelType);
            var queryString = actionContext.Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
            if (filterable && queryString.ContainsKey("filter"))
            {
                BindFilterableFromUri(bindingContext.Model as IFilterableQuery, queryString["filter"]);
            }
            return true;
        }

        private void BindFilterableFromUri(IFilterableQuery query, string filterQuery)
        {
            var converter = new QueryStringFilterConverter();
            query.Filter = (IFilter) converter.ConvertFrom(filterQuery);
        }
    }
}