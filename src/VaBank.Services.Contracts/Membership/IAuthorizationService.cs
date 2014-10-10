using System;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Contracts.Membership
{
    public interface IAuthorizationService
    {
        LoginResultModel Login(LoginCommand command);

        TokenModel CreateToken(CreateTokenCommand command);

        ProtectedTicketModel RevokeToken(IdentityQuery<string> query);

        ApplicationClientModel GetClient(IdentityQuery<string> query);

        LoginResultModel LoginById(IdentityQuery<Guid> query);
    }
}
