using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Common.Security;
using VaBank.Services.Contracts.Common.Validation;
using VaBank.Services.Contracts.Membership;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Models;
using VaBank.UI.Web.Api.Infrastructure.Converters;
using VaBank.UI.Web.Api.Infrastructure.Models;

namespace VaBank.UI.Web.Api.Infrastructure.Auth
{
    public class VabankAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        //TODO: common parts of grant refresh token and grant resource owner credentials to separate method

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var options = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            options.Converters.Insert(0, new HttpServiceErrorConverter());
            var identity = context.Ticket.Identity;
            if (identity == null)
            {
                context.SetError("Couldn't deserialize identity from refresh token.");
                return Task.FromResult<object>(null);
            }
            var userIdClaim = identity.FindFirst(ClaimTypes.Sid);
            if (userIdClaim == null)
            {
                context.SetError("Refresh token doesn't contain user id. Can't grant access.");
                return Task.FromResult<object>(null);
            }
            var container = context.OwinContext.GetAutofacLifetimeScope();
            var membershipService = container.Resolve<IAuthorizationService>();
            var userId = Guid.Parse(userIdClaim.Value);
            UserIdentityModel user = null;
            try
            {
                user = membershipService.RefreshLogin(new IdentityQuery<Guid>(userId));
            }
            catch (SecurityException ex)
            {
                var errorJson = JsonConvert.SerializeObject(ex.UserMessage, options);
                context.SetError("LoginFailure", errorJson);
                return Task.FromResult<object>(null);
            }
            if (user == null)
            {
                throw new InvalidOperationException("Refresh login returned null");
            }
            var newIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            foreach (var claimModel in user.Claims)
            {
                newIdentity.AddClaim(new Claim(claimModel.Type, claimModel.Value));
            }
            context.OwinContext.Set("vabank:user", user);
            var client = context.OwinContext.Get<ApplicationClientModel>("vabank:client");

            // Set CORS header
            context.Response.Headers.Set("Access-Control-Allow-Origin", client.AllowedOrigin);

            // Set state as validated
            context.Validated(newIdentity);
            var cookieIdentity = new ClaimsIdentity(identity.Claims, CookieAuthenticationDefaults.AuthenticationType);
            context.Request.Context.Authentication.SignIn(cookieIdentity);

            return base.GrantRefreshToken(context);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //Validate client information here
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

            var membershipService = container.Resolve<IAuthorizationService>();
            var client = membershipService.GetClient(new IdentityQuery<string>(context.ClientId));

            if (!client.Active)
            {
                context.SetError("Client is not activated.");
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == ApplicationClientTypeModel.NativeConfidential)
            {
                if (string.IsNullOrEmpty(clientSecret))
                {
                    context.SetError("Confidential clients should specify secrets.");
                    return Task.FromResult<object>(null);
                }
                if (VaBank.Common.Security.Hash.Compute(clientSecret) != client.Secret)
                {
                    context.SetError("Client secret is invalid.");
                    return Task.FromResult<object>(null);
                }
            }
            context.OwinContext.Set("vabank:client", client);
            context.Validated();
            return base.ValidateClientAuthentication(context);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //Validate user name and password here
            var container = context.OwinContext.GetAutofacLifetimeScope();
            var membershipService = container.Resolve<IAuthorizationService>();
            var options = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            options.Converters.Insert(0, new HttpServiceErrorConverter());
            var loginCommand = new LoginCommand {Login = context.UserName, Password = context.Password};
            UserIdentityModel user = null;
            try
            {
                user = membershipService.Login(loginCommand);
            }
            catch (ValidationException ex)
            {
                var error = new HttpServiceError(ex);
                context.SetError("LoginValidationError", JsonConvert.SerializeObject(error, options));
                return Task.FromResult<object>(null);
            }
            catch (SecurityException ex)
            {
                var errorJson = JsonConvert.SerializeObject(new HttpServiceError(ex), options);
                context.SetError("LoginFailure", errorJson);
                return Task.FromResult<object>(null);
            }
            if (user == null)
            {
                throw new InvalidOperationException("Service returned null for login command");
            }
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            foreach (var claimModel in user.Claims)
            {
                identity.AddClaim(new Claim(claimModel.Type, claimModel.Value));
            }
            context.OwinContext.Set("vabank:user", user);
            var client = context.OwinContext.Get<ApplicationClientModel>("vabank:client");

            // Set CORS header
            context.Response.Headers.Set("Access-Control-Allow-Origin", client.AllowedOrigin);

            // Set state as validated and set cookie
            context.Validated(identity);

            var cookieIdentity = new ClaimsIdentity(identity.Claims, CookieAuthenticationDefaults.AuthenticationType);
            context.Request.Context.Authentication.SignIn(cookieIdentity);
            return base.GrantResourceOwnerCredentials(context);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            //Serialize useful information to token here
            if (context.Identity != null)
            {
                var roles = context.Identity.FindAll(ClaimTypes.Role)
                   .Select(x => x.Value)
                   .ToArray();
                var userNameClaim = context.Identity.FindFirst(ClaimTypes.Name);
                var userIdClaim = context.Identity.FindFirst(ClaimTypes.Sid);
                if (userNameClaim == null || userIdClaim == null)
                {
                    throw new InvalidOperationException("User id or user name claim is missing from the identity.");
                }
                context.AdditionalResponseParameters.Add("roles", JsonConvert.SerializeObject(roles));
                context.AdditionalResponseParameters.Add("userName", userNameClaim.Value);
                context.AdditionalResponseParameters.Add("userId", userIdClaim.Value);           
            }
            return base.TokenEndpoint(context);
        }
    }
}