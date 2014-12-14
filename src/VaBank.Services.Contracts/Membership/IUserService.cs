using System;
using PagedList;
using VaBank.Common.Data;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Models;
using VaBank.Services.Contracts.Membership.Queries;

namespace VaBank.Services.Contracts.Membership
{
    public interface IUserService
    {
        IPagedList<UserBriefModel> GetUsers(UsersQuery query);

        UserBriefModel GetUser(IdentityQuery<Guid> id);

        UserProfileModel GetProfile(IdentityQuery<Guid> id);

        FullUserProfileModel GetFullProfile(IdentityQuery<Guid> id);

        UserBriefModel CreateUser(CreateUserCommand command);

        void UpdateUser(UpdateUserCommand command);

        UserMessage ChangePassword(ChangePasswordCommand command);

        void UpdateProfile(UpdateProfileCommand command);
    }
}
