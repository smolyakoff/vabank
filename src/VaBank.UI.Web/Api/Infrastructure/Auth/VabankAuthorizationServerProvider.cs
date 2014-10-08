using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac.Integration.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using VaBank.Services.Contracts.Membership;

namespace VaBank.UI.Web.Api.Infrastructure.Auth
{
    public class VabankAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            //TODO: what am i doing here?
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
            context.Response.Headers.Set("Access-Control-Allow-Origin", "https://google.com");
            context.Validated();

            return base.ValidateClientAuthentication(context);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //I'm validating username and password here
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, "johndoe"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Customer"));

            context.Validated(identity);
            return base.GrantResourceOwnerCredentials(context);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            //here I'm serializing additional data with token
            var roles = context.Identity.FindAll(ClaimTypes.Role)
                .Select(x => x.Value)
                .ToArray();
            var name = context.Identity.Name;
            var id = context.Identity.FindFirst(ClaimTypes.Sid).Value;

            context.AdditionalResponseParameters.Add("roles", JsonConvert.SerializeObject(roles));
            context.AdditionalResponseParameters.Add("userName", name);
            context.AdditionalResponseParameters.Add("userId", id);
            return base.TokenEndpoint(context);
        }
    }
}