using System;
using System.Web.Http;
using VaBank.Common.Data;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Commands;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Payments;
using VaBank.Services.Contracts.Payments.Queries;
using VaBank.UI.Web.Api.Infrastructure.Filters;

namespace VaBank.UI.Web.Api.Customer
{
    [Authorize(Roles = "Customer")]
    public class CustomerCardController : ApiController
    {
        private readonly ICardAccountService _cardAccountService;

        private readonly IPaymentStatisticsService _paymentStatisticsService;

        public CustomerCardController(ICardAccountService cardAccountService, IPaymentStatisticsService paymentStatisticsService)
        {
            Argument.NotNull(paymentStatisticsService, "paymentStatisticsService");
            if (cardAccountService == null)
            {
                throw new ArgumentNullException("cardAccountService");
            }
            _cardAccountService = cardAccountService;
            _paymentStatisticsService = paymentStatisticsService;
        }

        [Route("api/users/{id:guid}/cards")]
        [HttpGet]
        public IHttpActionResult Get([FromUri]IdentityQuery<Guid> userId)
        {
            return Ok(_cardAccountService.GetCustomerCards(userId));
        }

        [Route("api/cards/{id:guid}/settings")]
        [HttpPut]
        [Transaction]
        public IHttpActionResult UpdateSettings([FromUri]Guid id, UpdateCardSettingsCommand command)
        {
            command.CardId = id;
            return Ok(_cardAccountService.UpdateCardSettings(command));
        }

        [Route("api/cards/{id:guid}/block")]
        [HttpPost]
        [Transaction]
        public IHttpActionResult Block([FromUri]Guid id, SetCardBlockCommand command)
        {
            command.CardId = id;
            return Ok(_cardAccountService.SetCardBlock(command));
        }

        [Route("api/cards/{id:guid}/balance/{currencyISOName}")]
        [HttpGet]
        public IHttpActionResult Balance([FromUri] CardBalanceQuery query)
        {
            return Ok(_cardAccountService.GetCardBalance(query));
        }

        [Route("api/cards/{cardId:guid}/costs")]
        [HttpGet]
        public IHttpActionResult GetCostsByPaymentCategory([FromUri] PaymentCategoryCostsQuery query)
        {
            var stats = _paymentStatisticsService.GetCostsByPaymentCategory(query);
            return Ok(stats);
        }
    }
}