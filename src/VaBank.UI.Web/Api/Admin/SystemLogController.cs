using System.Web.Http;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Maintenance;
using VaBank.Services.Contracts.Maintenance.Commands;
using VaBank.Services.Contracts.Maintenance.Queries;
using VaBank.UI.Web.Api.Infrastructure.Filters;

namespace VaBank.UI.Web.Api.Admin
{
    [RoutePrefix("api/logs/system")]
    [Authorize(Roles = "Admin")]
    public class SystemLogController : ApiController
    {
        private readonly ILogService _logService;

        public SystemLogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Query(SystemLogQuery query)
        {
            var logs = _logService.GetSystemLogEntries(query);
            return Ok(logs);
        }

        [HttpGet]
        [Route("{id}/exception")]
        public IHttpActionResult Exception([FromUri]IdentityQuery<long> query)
        {
            var message = _logService.GetSystemLogException(query);
            return message == null ? (IHttpActionResult)NotFound() : Ok(message);
        }

        [HttpPost]
        [Route("clear")]
        [Transaction]
        public IHttpActionResult Clear(SystemLogClearCommand command)
        {
            var message = _logService.ClearSystemLog(command);
            return Ok(message);
        }

        [HttpGet]
        [Route("lookup")]
        public IHttpActionResult Lookup()
        {
            var lookup = _logService.GetSystemLogLookup();
            return Ok(lookup);
        }
    }
}