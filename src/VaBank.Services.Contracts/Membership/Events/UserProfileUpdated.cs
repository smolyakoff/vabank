using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Contracts.Membership.Events
{
    public class UserProfileUpdated : ApplicationEvent, IAuditedEvent
    {
        public UserProfileUpdated(UserProfileModel userProfileModel)
        {
            if (userProfileModel == null)
            {
                throw new ArgumentNullException("user profile");
            }
            ProfileModel = userProfileModel;
            Code = "USER_PROFILE_UPDATED";
            Description = string.Format("User profile changed.");
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

        public UserProfileModel ProfileModel { get; private set; }
    }
}
