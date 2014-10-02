using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace VaBank.UI.Web.Middleware.ExceptionHandling
{
    public class ExceptionHandlingMiddleware: OwinMiddleware
    {
        public ExceptionHandlingMiddleware(OwinMiddleware next)
            : base(next)
        {

        }

        public async override Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);
        }
    }
}