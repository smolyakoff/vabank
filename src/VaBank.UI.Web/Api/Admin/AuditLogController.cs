using System;
using System.Web.Http;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Maintenance;
using VaBank.Services.Contracts.Maintenance.Queries;

namespace VaBank.UI.Web.Api.Admin
{
    [RoutePrefix("api/logs/audit")]
    [Authorize(Roles="Admin")]
    public class AuditLogController : ApiController
    {
        private readonly ILogManagementService _logManagementService;

        public AuditLogController(ILogManagementService logManagementService)
        {
            _logManagementService = logManagementService;
        }

        [Route]
        [HttpGet]
        public IHttpActionResult Query(AuditLogQuery query)
        {
            var audit = _logManagementService.GetAuditLogEntries(query);
            return Ok(audit);
        }

        [Route("lookup")]
        [HttpGet]        
        public IHttpActionResult Lookup()
        {
            var lookup = _logManagementService.GetAuditLogLookup();
            return Ok(lookup);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult Operation([FromUri]IdentityQuery<Guid> query)
        {
            var entry = _logManagementService.GetAuditLogEntry(query);
            return entry == null ? (IHttpActionResult)NotFound() : Ok(entry);
        }
    }
}