using System.Net.Http;
using System.Web.Http.Filters;
using Autofac;
using Autofac.Integration.Owin;
using VaBank.Services.Contracts.Maintenance;

namespace VaBank.UI.Web.Api.Infrastructure.Filters
{
    public class ServiceOperationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var lifetimeScope = actionExecutedContext.Request.GetOwinContext().GetAutofacLifetimeScope();
            var operationService = lifetimeScope.Resolve<IOperationService>();
            if (operationService.HasCurrent)
            {
                var current = operationService.Current;
                operationService.Stop(current);
            }
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}