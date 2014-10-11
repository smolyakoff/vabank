using System;
using System.Linq;
using System.Security.Claims;
using VaBank.Common.Data;
using VaBank.Common.Data.Linq;
using VaBank.Core.Membership;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Models;
using VaBank.Services.Contracts.Membership.Queries;

namespace VaBank.Services.Membership
{
    internal static class MembershipExtensions
    {
        public static DbQuery<User> ToDbQuery(this LoginCommand command)
        {
            return DbQuery.For<User>().FilterBy(x => x.UserName == command.Login);
        }

        public static DbQuery<UserBriefModel> ToDbQuery(this UsersQuery query)
        {
            var spec = query.Roles.Aggregate(Specs.Active, (current, role) => current && Specs.InRole(role));
            return DbQuery.PagedFor<UserBriefModel>().FromClientQuery(query).AndFilterBy(spec);
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

        private static class Specs
        {
            public static readonly LinqSpec<UserBriefModel> Active = LinqSpec.For<UserBriefModel>(x => !x.Deleted);

            public static LinqSpec<UserBriefModel> InRole(string roleName)
            {
                return LinqSpec.For<UserBriefModel>(x => x.Claims.Any(y => y.Type == ClaimTypes.Role && y.Value == roleName));
            } 
        }
    }
}
