using System;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;

namespace VaBank.UI.Web.Api.Infrastructure.Auth
{
    public class VabankRefreshTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            //here is should create refresh token in the db
            var ticket = context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddSeconds(90);
            var serialized = context.SerializeTicket();
            context.SetToken(serialized);
            //base.Create(context);
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            var identity = new ClaimsIdentity("oauth");
            identity.AddClaim(new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, "johndoe"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            context.SetTicket(new AuthenticationTicket(identity, new AuthenticationProperties() {ExpiresUtc = DateTime.UtcNow.AddSeconds(90)}));
            //base.Receive(context);
        }
    }
}