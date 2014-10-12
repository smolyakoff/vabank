using System;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Contracts.Membership
{
    public interface IAuthorizationService
    {
        UserIdentityModel Login(LoginCommand command);

        TokenModel CreateToken(CreateTokenCommand command);

        ProtectedTicketModel RevokeToken(IdentityQuery<string> query);

        ApplicationClientModel GetClient(IdentityQuery<string> query);

        //Security note: should be only allowed for authenticated users
        UserIdentityModel RefreshLogin(IdentityQuery<Guid> query);
    }
}
