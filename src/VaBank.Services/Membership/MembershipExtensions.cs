using VaBank.Common.Data;
using VaBank.Core.Membership;
using VaBank.Services.Contracts.Membership;
using VaBank.Services.Contracts.Membership.Commands;

namespace VaBank.Services.Membership
{
    internal static class MembershipExtensions
    {
        public static IQuery ToDbQuery(this LoginCommand command)
        {
            return DbQuery.For<User>().FilterBy(x => x.UserName == command.Login);
        }
    }
}
