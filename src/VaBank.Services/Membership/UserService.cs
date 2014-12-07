using System;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using PagedList;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Membership.Entities;
using VaBank.Services.Common;
using VaBank.Services.Common.Exceptions;
using VaBank.Services.Common.Security;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Membership;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Events;
using VaBank.Services.Contracts.Membership.Models;
using VaBank.Services.Contracts.Membership.Queries;

namespace VaBank.Services.Membership
{
    public class UserService : BaseService, IUserService
    {
        private readonly UserManagementRepositories _db;

        public UserService(BaseServiceDependencies dependencies, UserManagementRepositories repositories) 
            : base(dependencies)
        {
            repositories.EnsureIsResolved();
            _db = repositories;
        }

        public IPagedList<UserBriefModel> GetUsers(UsersQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var usersPage = _db.Users.PartialProjectThenQueryPage<UserBriefModel>(
                    User.Spec.Active,
                    query.ToDbQuery());
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
                var user = _db.Users.PartialQueryIdentity(User.Spec.Active, id);
                return user == null ? null : user.ToModel<User, UserBriefModel>();
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
                var user = _db.Users.PartialQueryIdentity(User.Spec.Active, id);
                if (user == null || user.Profile == null)
                {
                    return null;
                }
                var profile = user.Profile.ToModel<UserProfile, UserProfileModel>();
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
                Commit();
                return user.ToModel<User, UserBriefModel>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't create user.", ex);
            }
        }

        public void UpdateUser(UpdateUserCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var user = _db.Users.Find(command.UserId);
                if (user == null || user.Deleted)
                {
                    throw NotFound.ExceptionFor<User>(command.UserId);
                }
                Mapper.Map(command, user);
                var role = UserClaim.CreateRole(command.UserId, command.Role);
                var existingRoles = user.Claims.Where(x => x.Type == ClaimTypes.Role).ToList();
                foreach (var existingRole in existingRoles)
                {
                    user.Claims.Remove(existingRole);
                }
                user.Claims.Add(role);
                if (command.ChangePassword)
                {
                    user.UpdatePassword(command.Password);
                }
                Mapper.Map(command, user.Profile);
                Commit();
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't update user.", ex);
            }
        }

        public void UpdateProfile(UpdateProfileCommand command)
        {
            EnsureIsValid(command);
            EnsureIsSecure(command, new UserCommandValidator(Identity));
            try
            {
                var user = _db.Users.Find(command.UserId);
                if (user == null || user.Deleted)
                {
                    throw NotFound.ExceptionFor<User>(command.UserId);
                }
                if (user.Profile == null)
                {
                    throw NotFound.ExceptionFor<UserProfile>(command.UserId);
                }
                
                Mapper.Map(command, user.Profile);
                //TODO: When phone number is changing should validate sms code
                if (!string.IsNullOrEmpty(command.PhoneNumber))
                {
                    user.Profile.PhoneNumberConfirmed = true;
                }
                _db.UserProfiles.Update(user.Profile);
                Commit();
                Publish(new UserProfileUpdated(Operation.Id, user.Profile.ToModel<UserProfile, UserProfileModel>()));
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't update profile.", ex);
            }
        }

        public UserMessage ChangePassword(ChangePasswordCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var user = _db.Users.Find(command.UserId);
                if (user == null || user.Deleted)
                {
                    throw NotFound.ExceptionFor<User>(command.UserId);
                }
                if (!user.ValidatePassword(command.CurrentPassword))
                {
                    throw AccessFailure.ExceptionBecause(AccessFailureReason.BadCredentials);
                }
                user.UpdatePassword(command.NewPassword);
                _db.Tokens.Delete(DbQuery.For<ApplicationToken>().FilterBy(x => x.User.Id == command.UserId));
                Commit();
                return UserMessage.Resource(() => Messages.PasswordChanged);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't change password.", ex);
            }
        }
    }
}
