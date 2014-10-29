using Newtonsoft.Json;
using System;
using VaBank.Common.Util;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Contracts.Membership.Events
{
    public class UserLoginFailed: ApplicationEvent, IAuditedEvent
    {

        public UserLoginFailed(Guid operationId, UserIdentityModel user)
        {
            Assert.NotNull("user", user);
            OperationId = operationId;
            Code = "LOGIN_FAILED";
            Description = string.Format("User [{0}] could not to log in.", user.UserName);
            Data = null;
        }

        [JsonConstructor]
        protected UserLoginFailed() { }

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
