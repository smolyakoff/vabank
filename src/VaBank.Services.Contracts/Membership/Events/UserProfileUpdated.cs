using Newtonsoft.Json;
using System;
using VaBank.Common.Util;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Contracts.Membership.Events
{
    public class UserProfileUpdated : ApplicationEvent, IAuditedEvent
    {
        public UserProfileUpdated(Guid operationId, UserProfileModel userProfile)
        {
            Assert.NotNull("userProfile", userProfile);
            OperationId = operationId;
            Code = "USER_PROFILE_UPDATED";
            Description = string.Format("User profile [{0}] changed.", userProfile.UserId);
            Data = null;
        }

        [JsonConstructor]
        protected UserProfileUpdated() { }

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
