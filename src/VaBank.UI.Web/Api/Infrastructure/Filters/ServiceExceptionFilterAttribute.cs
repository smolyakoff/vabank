using System;
using System.Net.Http;
using System.Web.Http.Filters;
using VaBank.Services.Contracts.Common;
using VaBank.UI.Web.Api.Infrastructure.Models;

namespace VaBank.UI.Web.Api.Infrastructure.Filters
{
    public class ServiceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = (dynamic) actionExecutedContext.Exception;
            this.Handle(exception, actionExecutedContext);
        }

        private void Handle(ServiceException exception, HttpActionExecutedContext context)
        {
            var error = new HttpServiceError(exception);
            context.Response = context.Request.CreateErrorResponse(error.StatusCode, error.HttpError);
        }

        private void Handle(Exception exception, HttpActionExecutedContext context)
        {
            base.OnException(context);
        }
    }
}