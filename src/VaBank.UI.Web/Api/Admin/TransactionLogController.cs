using System;
using System.Web.Http;
using VaBank.Common.Data;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Maintenance;
using VaBank.Services.Contracts.Maintenance.Queries;

namespace VaBank.UI.Web.Api.Admin
{
    [RoutePrefix("api/logs/transaction")]
    [Authorize(Roles = "Admin")]
    public class TransactionLogController : ApiController
    {
        private readonly ILogService _logService;

        public TransactionLogController(ILogService logService)
        {
            Argument.NotNull(logService, "logService");
            _logService = logService;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Query(TransactionLogQuery query)
        {
            return Ok(_logService.GetTransactionLogEntries(query));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IHttpActionResult Entry([FromUri] IdentityQuery<Guid> query)
        {
            return Ok(_logService.GetTransactionLogEntry(query));
        }
    }
}