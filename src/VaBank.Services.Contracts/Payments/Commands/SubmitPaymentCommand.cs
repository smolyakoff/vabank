using System;
using Newtonsoft.Json.Linq;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Processing.Commands;

namespace VaBank.Services.Contracts.Payments.Commands
{
    public class SubmitPaymentCommand : ICardWithdrawalCommand
    {
        public string TemplateCode { get; set; }

        public Guid FromCardId { get; set; }

        public SecurityCodeModel SecurityCode { get; set; }

        public JObject Form { get; set; }

        public decimal Amount
        {
            get { return Form["amount"].Value<decimal>(); }
        }
    }
}
