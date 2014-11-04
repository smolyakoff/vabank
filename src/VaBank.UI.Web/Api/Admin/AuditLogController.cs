using System;
using System.Web.Http;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Maintenance;
using VaBank.Services.Contracts.Maintenance.Queries;

namespace VaBank.UI.Web.Api.Admin
{
    [RoutePrefix("api/logs/audit")]
    [Authorize(Roles="Admin")]
    public class AuditLogController : ApiController
    {
        private readonly ILogService _logService;

        public AuditLogController(ILogService logService)
        {
            _logService = logService;
        }

        [Route]
        [HttpGet]
        public IHttpActionResult Query(AuditLogQuery query)
        {
            var audit = _logService.GetAuditLogEntries(query);
            return Ok(audit);
        }

        [Route("lookup")]
        [HttpGet]        
        public IHttpActionResult Lookup()
        {
            var lookup = _logService.GetAuditLogLookup();
            return Ok(lookup);
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult Operation([FromUri]IdentityQuery<Guid> query)
        {
            var entry = _logService.GetAuditLogEntry(query);
            return entry == null ? (IHttpActionResult)NotFound() : Ok(entry);
        }
    }
}