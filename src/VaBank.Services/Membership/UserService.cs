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
        private readonly UserManagementDependencies _deps;

        public UserService(BaseServiceDependencies dependencies, UserManagementDependencies deps) 
            : base(dependencies)
        {
            dependencies.EnsureIsResolved();
            _deps = deps;
        }

        public IPagedList<UserBriefModel> GetUsers(UsersQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var usersPage = _deps.Users.PartialProjectThenQueryPage<UserBriefModel>(
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
                var user = _deps.Users.PartialQueryIdentity(User.Spec.Active, id);
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
                var user = _deps.Users.PartialQueryIdentity(User.Spec.Active, id);
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

        public FullUserProfileModel GetFullProfile(IdentityQuery<Guid> id)
        {
            EnsureIsValid(id);
            try
            {
                var user = _deps.Users.PartialQueryIdentity(User.Spec.Active, id);
                if (user == null)
                {
                    return null;
                }
                var paymentProfile = _deps.PaymentProfiles.Find(id.Id);
                return new FullUserProfileModel
                {
                    User = user.ToModel<UserBriefModel>(),
                    Profile = user.Profile == null ? null : user.Profile.ToModel<UserProfileModel>(),
                    PaymentProfile = paymentProfile == null ? null : paymentProfile.ToModel<UserPaymentProfileModel>()
                };
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get full profile.", ex);
            }
        }

        public UserBriefModel CreateUser(CreateUserCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var user = command.ToEntity<CreateUserCommand, User>();
                _deps.Users.Create(user);
                if (!user.IsAdmin)
                {
                    var paymentProfile = _deps.UserPaymentProfileFactory.Create(user, command.Address, command.FullName);
                    _deps.PaymentProfiles.Create(paymentProfile);
                }
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
                var user = _deps.Users.Find(command.UserId);
                if (user == null || user.Deleted)
                {
                    throw NotFound.ExceptionFor<User>(command.UserId);
                }
                var paymentProfile = _deps.PaymentProfiles.Find(command.UserId);
                if (command.Role == UserClaim.Roles.Admin && paymentProfile != null)
                {
                    _deps.PaymentProfiles.Delete(paymentProfile);
                }
                else if (paymentProfile == null)
                {
                    paymentProfile = _deps.UserPaymentProfileFactory.Create(user, command.Address, command.FullName);
                    _deps.PaymentProfiles.Create(paymentProfile);
                }
                else
                {
                    Mapper.Map(command, paymentProfile);
                    _deps.PaymentProfiles.Update(paymentProfile);
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
                var user = _deps.Users.Find(command.UserId);
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
                _deps.UserProfiles.Update(user.Profile);
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

        public UserMessage UnlockUser(IdentityQuery<Guid> userId)
        {
            EnsureIsValid(userId);
            try
            {
                var user = _deps.Users.Find(userId.Id);
                if (user == null || user.Deleted)
                {
                    throw NotFound.ExceptionFor<User>(userId.Id);
                }
                _deps.UserLockoutPolicy.Unblock(user);
                Commit();
                return UserMessage.Resource(() => Messages.UserSuccessfullyUnblocked);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't unblock user.", ex);
            }
        }

        public UserMessage ChangePassword(ChangePasswordCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var user = _deps.Users.Find(command.UserId);
                if (user == null || user.Deleted)
                {
                    throw NotFound.ExceptionFor<User>(command.UserId);
                }
                if (!user.ValidatePassword(command.CurrentPassword))
                {
                    throw AccessFailure.ExceptionBecause(AccessFailureReason.BadCredentials);
                }
                user.UpdatePassword(command.NewPassword);
                _deps.Tokens.Delete(DbQuery.For<ApplicationToken>().FilterBy(x => x.User.Id == command.UserId));
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
