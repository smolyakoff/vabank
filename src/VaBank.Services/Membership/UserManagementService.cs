using System;
using FluentValidation;
using PagedList;
using VaBank.Core.Common;
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
            throw new NotImplementedException();
        }

        public UserProfileModel GetProfile(IdentityQuery<Guid> id)
        {
            throw new NotImplementedException();
        }

        public UserBriefModel CreateUser(CreateUserCommand command)
        {
            throw new NotImplementedException();
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
