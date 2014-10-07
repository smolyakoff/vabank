using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.WebSockets;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Membership;

namespace VaBank.UI.Web.Api.Infrastructure.Auth
{
    public class VabankAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            return base.GrantRefreshToken(context);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //I'm validating client id here
            var container = context.OwinContext.GetAutofacLifetimeScope();
            string clientId, clientSecret;
            if (!context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                context.TryGetBasicCredentials(out clientId, out clientSecret);
            }
            if (context.ClientId == null)
            {
                context.SetError("Client id is not specified.");
                return Task.FromResult<object>(null);
            }

            //var membershipService = container.Resolve<IMembershipService>();
            //membershipService.GetClient(new IdentityQuery<string> {Id = context.ClientId});
            var client = new ApplicationClientModel();
            context.Validated();

            return base.ValidateClientAuthentication(context);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //I'm validating username and password here
            var identity = new ClaimsIdentity("oauth");
            identity.AddClaim(new Claim(ClaimTypes.Role, "customer"));
            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties(new Dictionary<string, string>(){{".role1", "admin"}})
            {
                IssuedUtc = DateTime.UtcNow
            });
            context.Validated(ticket);


            return base.GrantResourceOwnerCredentials(context);
        }
    }
}