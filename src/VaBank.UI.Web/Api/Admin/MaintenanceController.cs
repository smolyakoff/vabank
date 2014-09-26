using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace VaBank.UI.Web.Api.Admin
{
    [RoutePrefix("api/maintenance")]
    public class MaintenanceController : ApiController
    {
        [HttpGet]
        [Route("keep-alive")]
        public IHttpActionResult KeepAlive()
        {
            var message = new HttpResponseMessage(HttpStatusCode.OK);
            message.Headers.Clear();
            return ResponseMessage(message);
        }
    }
}