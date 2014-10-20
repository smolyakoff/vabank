using System.Net.Http;
using System.Web.Http.Filters;
using Autofac;
using Autofac.Integration.Owin;
using VaBank.Core.App;

namespace VaBank.UI.Web.Api.Infrastructure.Filters
{
    public class ServiceOperationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var lifetimeScope = actionExecutedContext.Request.GetOwinContext().GetAutofacLifetimeScope();
            var operationProvider = lifetimeScope.ResolveNamed<IOperationProvider>("Service");
            var operationRepository = lifetimeScope.Resolve<IOperationRepository>();
            if (operationProvider.HasCurrent)
            {
                var current = operationProvider.GetCurrent();
                operationRepository.Stop(current);
            }
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}