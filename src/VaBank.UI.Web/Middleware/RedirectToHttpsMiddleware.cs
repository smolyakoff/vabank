using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
            }

            return _next.Invoke(context);
        }
    }
}