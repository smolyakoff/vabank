using System;
using System.Collections.Generic;
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
            var spec = Specs.Active;
            if (query.Roles != null && query.Roles.Length > 0)
            {
                spec = spec && Specs.HasAtLeastOneRoleFrom(query.Roles);
            }
            return DbQuery.PagedFor<UserBriefModel>().FromClientQuery(query).AndFilterBy(spec);
        }

        public static UserMessage ToUserMessage(this AccessFailureReason failure)
        {
            switch (failure)
            {
                case AccessFailureReason.UserBlocked:
                    return UserMessage.Resource(() => Messages.UserBlocked);
                case AccessFailureReason.UserDeleted:
                    return UserMessage.Resource(() => Messages.UserDeleted);
                case AccessFailureReason.BadCredentials:
                    return UserMessage.Resource(() => Messages.InvalidCredentials);
                default:
                    throw new InvalidOperationException("Can't get user message for unknown LoginFailureReason type.");
            }
        }


        private static class Specs
        {
            public static readonly LinqSpec<UserBriefModel> Active = LinqSpec.For<UserBriefModel>(x => !x.Deleted);

            public static LinqSpec<UserBriefModel> HasAtLeastOneRoleFrom(IEnumerable<string> roleNames)
            {
                return LinqSpec.For<UserBriefModel>(x => x.Claims.Any(y => y.Type == ClaimTypes.Role && roleNames.Contains(y.Value)));
            } 

            public static LinqSpec<UserBriefModel> InRole(string roleName)
            {
                return LinqSpec.For<UserBriefModel>(x => x.Claims.Any(y => y.Type == ClaimTypes.Role && y.Value == roleName));
            } 
        }
    }
}
