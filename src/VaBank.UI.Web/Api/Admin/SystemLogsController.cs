using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using VaBank.Services.Contracts.Admin.Maintenance;
using VaBank.Services.Contracts.Common.Queries;

namespace VaBank.UI.Web.Api.Admin
{
    [RoutePrefix("api/logs/system")]
    public class SystemLogsController : ApiController
    {
        private readonly ILogManagementService _logManagementService;

        public SystemLogsController(ILogManagementService logManagementService)
        {
            _logManagementService = logManagementService;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Query([ModelBinder] SystemLogQuery query)
        {
            var logs = _logManagementService.GetSystemLogEntries(query);
            return Ok(logs);
        }

        public IHttpActionResult Exception([ModelBinder] IdentityQuery<long> query)
        {
            var message = _logManagementService.GetSystemLogException(query);
            return message == null ? (IHttpActionResult)NotFound() : Ok(message);
        }

        [HttpPost]
        [Route("clear")]
        public IHttpActionResult Clear([ModelBinder] SystemLogQuery query)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("lookup")]
        public IHttpActionResult Lookup()
        {
            var lookup = _logManagementService.GetSystemLogLookup();
            return Ok(lookup);
        }
    }
}