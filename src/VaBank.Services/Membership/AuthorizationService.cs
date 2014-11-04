using System;
using VaBank.Common.Data;
using VaBank.Core.Membership.Access;
using VaBank.Core.Membership.Entities;
using VaBank.Services.Common;
using VaBank.Services.Common.Exceptions;
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
        private readonly AuthorizationServiceDepenencies _deps;

        public AuthorizationService(BaseServiceDependencies dependencies,
            AuthorizationServiceDepenencies serviceDepenencies)
            : base(dependencies)
        {
            serviceDepenencies.EnsureIsResolved();
            _deps = serviceDepenencies;
        }

        public UserIdentityModel Login(LoginCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var user = _deps.Users.QueryOne(command.ToDbQuery());
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
                Commit();
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
                var client = _deps.ApplicationClients.Find(command.ClientId);
                if (client == null)
                {
                    throw NotFound.ExceptionFor<ApplicationClient>(command.ClientId);
                }
                var user = _deps.Users.Find(command.UserId);
                if (user == null)
                {
                    throw NotFound.ExceptionFor<User>(command.UserId);
                }
                token.Client = client;
                token.User = user;
                _deps.ApplicationTokens.Create(token);
                Commit();
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
                var token = _deps.ApplicationTokens.GetAndDelete(query.Id);
                return token == null ? null : new ProtectedTicketModel(token.ProtectedTicket);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get revoke token.", ex);
            }
        }

        public ApplicationClientModel GetClient(IdentityQuery<string> query)
        {
            EnsureIsValid(query);
            try
            {
                var client =
                    _deps.ApplicationClients.ProjectIdentity<string, ApplicationClient, ApplicationClientModel>(query);
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
                var user = _deps.Users.QueryIdentity(query);
                if (user == null)
                {
                    throw AccessFailure.ExceptionBecause(AccessFailureReason.BadCredentials);
                }
                var reason = VerifyAccess(user);
                if (reason == null)
                {
                    return user.ToModel<User, UserIdentityModel>();
                }
                Commit();
                throw AccessFailure.ExceptionBecause(reason.Value);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get user.", ex);
            }
        }

        private AccessFailureReason? VerifyAccess(User user, string password = null)
        {
            var access = password == null
                ? _deps.UserAccessPolicy.VerifyAccess(user, null, false)
                : _deps.UserAccessPolicy.VerifyAccess(user, password, true);
            switch (access)
            {
                case AccessStatus.BadCredentials:
                    return AccessFailureReason.BadCredentials;
                case AccessStatus.Blocked:
                    return AccessFailureReason.UserBlocked;
                case AccessStatus.Deleted:
                    return AccessFailureReason.UserDeleted;
                default:
                    return null;
            }
        }
    }
}
