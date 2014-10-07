using Microsoft.Owin;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace VaBank.UI.Web.Middleware
{
    public class ExceptionMiddleware: OwinMiddleware
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ExceptionMiddleware(OwinMiddleware next)
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
            _logger.Error(ex.Message, ex);
        }
    }
}