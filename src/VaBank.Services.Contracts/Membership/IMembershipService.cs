using VaBank.Services.Contracts.Common.Queries;

namespace VaBank.Services.Contracts.Membership
{
    public interface IMembershipService
    {
        LoginResultModel Login(LoginCommand command);

        TokenModel CreateToken(CreateTokenCommand command);

        TokenModel GetToken(IdentityQuery<string> query);

        bool RemoveToken(IdentityQuery<string> query);

        ApplicationClientModel GetClient(IdentityQuery<string> query);
    }
}
