using Microsoft.Owin;
using System.Threading.Tasks;

namespace VaBank.UI.Web.Middleware
{
    public class RedirectToHttpsMiddleware: OwinMiddleware
    {
        private readonly OwinMiddleware _next;

        public RedirectToHttpsMiddleware(OwinMiddleware next)
            : base(next)
        {
            _next = next;
        }

        public override Task Invoke(IOwinContext context)
        {
            if (!context.Request.IsSecure)
            {
                var secureUri = context.Request.Uri.ToString().Replace("http", "https");
                context.Response.Redirect(secureUri);
                return Task.FromResult(new object());
            }
            return _next.Invoke(context);
        }
    }
}