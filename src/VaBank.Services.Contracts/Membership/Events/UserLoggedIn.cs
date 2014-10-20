using System;
using Newtonsoft.Json;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Contracts.Membership.Events
{
    public class UserLoggedIn : ApplicationEvent, IAuditedEvent
    {
        public UserLoggedIn(Guid operationId, UserIdentityModel user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            OperationId = operationId;
            Code = "LOGIN";
            Description = string.Format("User [{0}] successfully logged in.", user.UserName);
            Data = null;
        }

        [JsonConstructor]
        protected UserLoggedIn()
        {
        }

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
