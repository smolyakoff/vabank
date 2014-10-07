using VaBank.Services.Contracts.Common.Queries;

namespace VaBank.Services.Contracts.Membership
{
    public interface IMembershipService
    {
        LoginResultModel Login(LoginCommand command);

        TokenModel CreateToken(CreateTokenCommand command);

        TokenModel GetToken(IdentityQuery<string> id);

        bool RemoveToken(IdentityQuery<string> id);

        ApplicationClientModel GetClient(IdentityQuery<string> id);
    }
}
