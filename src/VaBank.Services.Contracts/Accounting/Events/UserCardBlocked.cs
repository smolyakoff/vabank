using Newtonsoft.Json;
using System;
using VaBank.Common.Util;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Infrastructure.Sms;

namespace VaBank.Services.Contracts.Accounting.Events
{
    public class UserCardBlocked: ApplicationEvent, IAuditedEvent, ISmsEvent
    {
        public UserCardBlocked(CustomerCardModel card, Guid operationId)
        {
            Assert.NotNull("card", card);
            OperationId = operationId;
            Code = "USER_CARD_BLOCKED";
            Description = string.Format("User card [{0}] blocked.", card.CardId);
            Data = null;
            Card = card;
        }

        [JsonConstructor]
        protected UserCardBlocked() { }

        [JsonProperty]
        public CustomerCardModel Card { get; private set; }

        [JsonProperty]
        public Guid OperationId { get; private set; }

        [JsonProperty]
        public string Code { get; private set; }

        [JsonProperty]
        public string Description { get; private set; }

        [JsonProperty]
        public object Data { get; private set; }
    }
}
