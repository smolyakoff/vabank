using System;
using VaBank.Common.Data;
using VaBank.Core.Membership;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Membership;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Security;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Events;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Membership
{
    public class AuthorizationService : BaseService, IAuthorizationService
    {
        private readonly AuthorizationRepositories _db;

        public AuthorizationService(BaseServiceDependencies dependencies, AuthorizationRepositories repositories) 
            : base(dependencies)
        {
            repositories.EnsureIsResolved();
            _db = repositories;
        }

        public UserIdentityModel Login(LoginCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var user = _db.Users.QueryOne(command.ToDbQuery());
                if (user == null)
                {
                    throw AccessFailure.ExceptionBecause(AccessFailureReason.BadCredentials);
                }
                var reason = VerifyAccess(user, command.Password);
                if (reason == null)
                {
                    var model = user.ToModel<User, UserIdentityModel>();
                    Publish(new UserLoggedIn(Operation.Id, model));
                    return model;
                }
                ++user.AccessFailedCount;
                if (reason == AccessFailureReason.BadCredentials)
                {
                    //TODO: this is domain logic
                    if (user.AccessFailedCount > 3)
                    {
                        user.LockoutEnabled = true;
                        user.LockoutEndDateUtc = DateTime.MaxValue;
                    }
                }
                UnitOfWork.Commit();
                Publish(new UserLoginFailed(Operation.Id, user.ToModel<User, UserIdentityModel>()));
                throw AccessFailure.ExceptionBecause(reason.Value);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't login user.", ex);
            }
        }

        public TokenModel CreateToken(CreateTokenCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var token = command.ToEntity<CreateTokenCommand, ApplicationToken>();
                var client = _db.ApplicationClients.Find(command.ClientId);
                if (client == null)
                {
                    throw new InvalidOperationException("Token is referencing to invalid client.");
                }
                var user = _db.Users.Find(command.UserId);
                if (user == null)
                {
                    throw new InvalidOperationException("Token is referencing to invalid user.");
                }
                token.Client = client;
                token.User = user;
                _db.ApplicationTokens.Create(token);
                UnitOfWork.Commit();
                return command.ToClass<CreateTokenCommand, TokenModel>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't create application token.", ex);
            }
        }

        public ProtectedTicketModel RevokeToken(IdentityQuery<string> query)
        {
            EnsureIsValid(query);
            try
            {
                var token = _db.ApplicationTokens.GetAndDelete(query.Id);
                return token == null ? null : new ProtectedTicketModel(token.ProtectedTicket);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get revoke token.", ex);
            }
        }

        public ApplicationClientModel GetClient(IdentityQuery<string> query)
        {
            //TODO: maybe use caching for clients?
            EnsureIsValid(query);
            try
            {
                var client = _db.ApplicationClients.ProjectIdentity<string, ApplicationClient, ApplicationClientModel>(query);
                return client;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get application client.", ex);
            }
        }

        public UserIdentityModel RefreshLogin(IdentityQuery<Guid> query)
        {
            EnsureIsValid(query);
            try
            {
                var user = _db.Users.QueryIdentity(query);
                if (user == null)
                {
                    throw AccessFailure.ExceptionBecause(AccessFailureReason.BadCredentials);
                }
                var reason = VerifyAccess(user);
                if (reason == null)
                {
                    return user.ToModel<User, UserIdentityModel>();
                }
                user.AccessFailedCount++;
                UnitOfWork.Commit();
                throw AccessFailure.ExceptionBecause(reason.Value);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get user.", ex);
            }
        }

        private AccessFailureReason? VerifyAccess(User user, string password)
        {
            var reason = VerifyAccess(user);
            if (reason != null)
            {
                return reason;
            }
            if (!Password.Validate(user.PasswordHash, user.PasswordSalt, password))
            {
                return AccessFailureReason.BadCredentials;
            }
            return null;
        }

        //TODO: this is domain logic, move to core
        private AccessFailureReason? VerifyAccess(User user)
        {
            if (user.Deleted)
            {
                return AccessFailureReason.UserDeleted;
            }
            if (user.LockoutEnabled && user.LockoutEndDateUtc > DateTime.UtcNow)
            {
                return AccessFailureReason.UserBlocked;
            }
            if (user.LockoutEnabled && user.LockoutEndDateUtc < DateTime.UtcNow)
            {
                user.LockoutEnabled = false;
                user.LockoutEndDateUtc = null;
            }
            return null;
        }
    }
}
