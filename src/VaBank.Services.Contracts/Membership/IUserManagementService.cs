using System;
using PagedList;
using VaBank.Services.Contracts.Common.Models;
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

        bool UpdateUser(UpdateUserCommand command);

        UserMessage ChangePassword(ChangePasswordCommand command);

        bool UpdateProfile(UpdateProfileCommand command);
    }
}
