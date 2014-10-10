using System;
using VaBank.Common.Data;
using VaBank.Core.Membership;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Membership
{
    internal static class MembershipExtensions
    {
        public static IQuery ToDbQuery(this LoginCommand command)
        {
            return DbQuery.For<User>().FilterBy(x => x.UserName == command.Login);
        }

        public static UserMessage UserMessage(this LoginFailureReason reason)
        {
            switch (reason)
            {
                case LoginFailureReason.UserBlocked:
                    return new UserMessage(Messages.UserBlocked);
                case LoginFailureReason.UserDeleted:
                    return new UserMessage(Messages.UserDeleted);
                case LoginFailureReason.BadCredentials:
                    return new UserMessage(Messages.InvalidCredentials);
                default:
                    throw new InvalidOperationException("Can't get user message for unknown LoginFailureReason type.");
            }
        }
    }
}
