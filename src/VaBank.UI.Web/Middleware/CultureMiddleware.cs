using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace VaBank.UI.Web.Middleware
{
    public class CultureMiddleware : OwinMiddleware
    {
        private readonly OwinMiddleware _next;

        public CultureMiddleware(OwinMiddleware next) : base(next)
        {
            _next = next;
        }

        public override Task Invoke(IOwinContext context)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
            if (_next != null)
            {
                return _next.Invoke(context);
            }
            return Task.Run(() => { });
        }
    }
}