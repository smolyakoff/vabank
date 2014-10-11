using System;
using System.Web.Http;

namespace VaBank.UI.Web.Api.Common
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult Get()
        {
            throw new NotImplementedException();
        }
    }
}