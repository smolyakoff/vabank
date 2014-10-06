using Microsoft.Owin;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace VaBank.UI.Web.Middleware.ExceptionHandling
{
    public class ExceptionHandlingMiddleware: OwinMiddleware
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ExceptionHandlingMiddleware(OwinMiddleware next)
            : base(next)
        {

        }

        public async override Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch(Exception ex)
            {
                ProcessException(ex);
            }
        }

        private void ProcessException(Exception ex)
        {
            logger.Error(ex);
        }
    }
}