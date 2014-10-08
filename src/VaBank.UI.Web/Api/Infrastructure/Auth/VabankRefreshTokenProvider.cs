using Microsoft.Owin.Security.Infrastructure;

namespace VaBank.UI.Web.Api.Infrastructure.Auth
{
    public class VabankRefreshTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            //here is should create refresh token in the db
            base.Create(context);
        }
    }
}