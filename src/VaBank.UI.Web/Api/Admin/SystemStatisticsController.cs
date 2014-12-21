using System.Web.Http;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Maintenance;
using VaBank.Services.Contracts.Maintenance.Queries;

namespace VaBank.UI.Web.Api.Admin
{
    [RoutePrefix("api/system/stats")]
    [Authorize(Roles = "Admin")]
    public class SystemStatisticsController : ApiController
    {
        private readonly ISystemStatisticsService _systemStatisticsService;

        public SystemStatisticsController(ISystemStatisticsService systemStatisticsService)
        {
            Argument.NotNull(systemStatisticsService, "systemStatustucsService");
            _systemStatisticsService = systemStatisticsService;
        }

        [HttpGet]
        [Route("info")]
        public IHttpActionResult Info()
        {
            return Ok(_systemStatisticsService.GetSystemInfo());
        }

        [HttpGet]
        [Route("transactions")]
        public IHttpActionResult Transactions([FromUri]TransactionStatisticsQuery query)
        {
            query = query ?? new TransactionStatisticsQuery();
            return Ok(_systemStatisticsService.GetProcessedTransactionStatistics(query));
        }
    }
}
