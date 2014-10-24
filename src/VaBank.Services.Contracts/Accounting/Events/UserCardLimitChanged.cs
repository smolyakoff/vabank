using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Events;

namespace VaBank.Services.Contracts.Accounting.Events
{
    public class UserCardLimitChanged : ApplicationEvent, IAuditedEvent
    {
        public UserCardLimitChanged(CardAccountModel accountModel)
        {
            if (accountModel == null)
            {
                throw new ArgumentNullException("user card");
            }
            CardAccountModel = accountModel;
            Code = "USER_CARD_LIMIT_CHANGED";
            Description = string.Format("User card [{0}] limits changed.", accountModel.CardNo);
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

        public CardAccountModel CardAccountModel { get; private set; }
    }
}
