using Microsoft.Owin.Security.Infrastructure;

namespace VaBank.UI.Web.Api.Infrastructure.Auth
{
    public class VabankTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            base.Create(context);
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            base.Receive(context);
        }
    }
}