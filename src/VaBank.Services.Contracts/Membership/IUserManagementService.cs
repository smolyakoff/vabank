using System;
using PagedList;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Models;
using VaBank.Services.Contracts.Membership.Queries;

namespace VaBank.Services.Contracts.Membership
{
    public interface IUserManagementService
    {
        IPagedList<UserBriefModel> GetUsers(UsersQuery query);

        UserBriefModel GetUser(IdentityQuery<Guid> id);

        UserProfileModel GetProfile(IdentityQuery<Guid> id);

        UserBriefModel CreateUser(CreateUserCommand command);

        void UpdateUser(UpdateUserCommand command);

        void ChangePassword(ChangePasswordCommand command);
    }
}
