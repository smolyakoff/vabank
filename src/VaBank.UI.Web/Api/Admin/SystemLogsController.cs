using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Admin.Maintenance;
using VaBank.UI.Web.Api.Infrastructure.ModelBinders;

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
        public IHttpActionResult Query([ModelBinder(typeof(QueryModelBinder))]SystemLogQuery query)
        {
            var logs = _logManagementService.GetSystemLogEntries(query);
            return Ok(logs);
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