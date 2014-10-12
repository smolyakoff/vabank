using System.Web.Http;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using System;
using System.Threading.Tasks;

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
                _logger.Error("Last chance exception caught.", ex);
                var headers = context.Request.Headers;
                // if server accepts json - just return json error
                if (headers.ContainsKey("Accept") && headers["Accept"].Contains("application/json"))
                {
                    var includeDetail = context.Request.LocalIpAddress == context.Request.RemoteIpAddress;
                    var message = new HttpError(ex, includeDetail);
                    var serializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };
                    context.Response.StatusCode = 500;
                    context.Response.Write(JsonConvert.SerializeObject(message, serializerSettings));
                }
                // otherwise - let the owin to return html
                else
                {
                    throw;
                }
            }
        }
    }
}