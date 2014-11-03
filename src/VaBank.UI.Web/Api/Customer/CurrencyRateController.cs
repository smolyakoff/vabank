using System.Web.Http;
using VaBank.Services.Contracts.Processing;

namespace VaBank.UI.Web.Api.Customer
{
    [RoutePrefix("api/currencies/rates")]
    public class CurrencyRateController : ApiController
    {
        private readonly ICurrencyRateService _currencyRateService;

        [HttpGet]
        [Route]
        public IHttpActionResult GetAllRates()
        {
            return Ok(_currencyRateService.GetAllRates());
        }
    }
}
