using VaBank.Common.Data;
using VaBank.Core.Membership;
using VaBank.Services.Contracts.Membership;

namespace VaBank.Services.Membership
{
    internal static class MembershipExtensions
    {
        public static IQuery QueryUser(this LoginCommand command)
        {
            return DbQuery.For<User>().FilterBy(x => x.UserName == command.Login);
        }
    }
}
