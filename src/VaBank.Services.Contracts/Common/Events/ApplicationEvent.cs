using System;
using System.Security.Claims;
using System.Threading;
using Newtonsoft.Json;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Contracts.Common.Events
{
    public abstract class ApplicationEvent : IApplicationEvent
    {
        protected ApplicationEvent()
        {
            TimestampUtc = DateTime.UtcNow;
            UserId = GetUserId();
        }

        [JsonProperty]
        public DateTime TimestampUtc { get; protected set; }

        [JsonProperty]
        public Guid? UserId { get; protected set; }

        private static Guid? GetUserId()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return null;
            }
            var id = claimsIdentity.FindFirst(ClaimModel.Types.UserId);
            if (id != null)
            {
                return Guid.Parse(id.Value);
            }
            return null;
        }
    }
}
