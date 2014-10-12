using System;
using FluentValidation;
using PagedList;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Common;
using VaBank.Core.Membership;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Membership;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Models;
using VaBank.Services.Contracts.Membership.Queries;

namespace VaBank.Services.Membership
{
    public class UserManagementService : BaseService, IUserManagementService
    {
        private readonly UserManagementRepositories _db;

        public UserManagementService(IUnitOfWork unitOfWork, IValidatorFactory validatorFactory, UserManagementRepositories repositories) 
            : base(unitOfWork, validatorFactory)
        {
            repositories.EnsureIsResolved();
            _db = repositories;
        }

        public IPagedList<UserBriefModel> GetUsers(UsersQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var usersPage = _db.Users.ProjectThenQueryPage<UserBriefModel>(query.ToDbQuery());
                return usersPage;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get users.", ex);
            }
        }

        public UserBriefModel GetUser(IdentityQuery<Guid> id)
        {
            EnsureIsValid(id);
            try
            {
                var user = _db.Users.ProjectIdentity<Guid, User, UserBriefModel>(id);
                return user;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get user.", ex);
            }
        }

        public UserProfileModel GetProfile(IdentityQuery<Guid> id)
        {
            EnsureIsValid(id);
            try
            {
                var profile = _db.UserProfiles.ProjectIdentity<Guid, UserProfile, UserProfileModel>(id);
                return profile;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get profile.", ex);
            }
        }

        public UserBriefModel CreateUser(CreateUserCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var user = command.ToEntity<CreateUserCommand, User>();
                _db.Users.Create(user);
                UnitOfWork.Commit();
                return user.ToModel<User, UserBriefModel>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't create user.", ex);
            }
        }

        public void UpdateUser(UpdateUserCommand command)
        {
            throw new NotImplementedException();
        }

        public void ChangePassword(ChangePasswordCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
