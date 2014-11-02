using Newtonsoft.Json;
using System;
using VaBank.Common.Util;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Common.Events;

namespace VaBank.Services.Contracts.Accounting.Events
{
    public class UserCardLimitChanged : ApplicationEvent, IAuditedEvent
    {
        public UserCardLimitChanged(CustomerCardModel customerCardModel, Guid operationId)
        {
            Assert.NotNull("customerCardModel", customerCardModel);
            Assert.IsFalse("operationId", operationId == Guid.Empty);
            OperationId = operationId;
            Code = "USER_CARD_LIMIT_CHANGED";
            Description = string.Format("User card [{0}] limits changed.", customerCardModel.CardId);
            Data = null;
        }

        [JsonConstructor]
        protected UserCardLimitChanged() { }

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
