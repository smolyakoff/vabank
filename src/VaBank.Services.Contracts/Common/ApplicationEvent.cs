using System;
using System.Security.Claims;
using System.Threading;
using VaBank.Common.Events;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Contracts.Common
{
    public abstract class ApplicationEvent : IEvent
    {
        protected ApplicationEvent()
        {
            DateUtc = DateTime.UtcNow;
            UserId = GetUserId();
        }

        public DateTime DateUtc { get; protected set; }

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
