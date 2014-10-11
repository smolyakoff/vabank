using System;
using System.Data.Entity;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin.Security.Infrastructure;
using NLog;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Membership;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.UI.Web.Api.Infrastructure.Auth
{
    public class VabankRefreshTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            //Create new application token in the database
            var client = context.OwinContext.Get<ApplicationClientModel>("vabank:client");
            if (client == null)
            {
                throw new InvalidOperationException("Client is not in owin context. Can't create refresh token");
            }
            var user = context.OwinContext.Get<UserIdentityModel>("vabank:user");
            if (user == null)
            {
                throw new InvalidOperationException("User is not in owin context. Can't create refresh token");
            }

            var container = context.OwinContext.GetAutofacLifetimeScope();
            var membershipService = container.Resolve<IAuthorizationService>();

            var tokenId = Guid.NewGuid().ToString("N");
            var token = new CreateTokenCommand
            {
                Id = VaBank.Common.Security.Hash.Compute(tokenId),
                ClientId = client.Id,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddSeconds(client.RefreshTokenLifetime),
                UserId = user.UserId
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;
            token.ProtectedTicket = context.SerializeTicket();
            membershipService.CreateToken(token);
            context.SetToken(tokenId);
            base.Create(context);
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            var container = context.OwinContext.GetAutofacLifetimeScope();
            var membershipService = container.Resolve<IAuthorizationService>();
            var hashedTokenId = VaBank.Common.Security.Hash.Compute(context.Token);
            var token = membershipService.RevokeToken(new IdentityQuery<string>(hashedTokenId));
            if (token != null)
            {
                context.DeserializeTicket(token.Value);
            }
            base.Receive(context);
        }
    }
}