using FluentValidation;
using System;
using VaBank.Core.Common;
using VaBank.Core.Membership;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Queries;
using VaBank.Services.Contracts.Membership;
using VaBank.Common.Data.Repositories;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Common.Security;
using VaBank.Services.Contracts.Membership.Commands;
using VaBank.Services.Contracts.Membership.Models;

namespace VaBank.Services.Membership
{
    public class AuthorizationService : BaseService, IAuthorizationService
    {
        private readonly AuthorizationRepositories _db;

        public AuthorizationService(IUnitOfWork unitOfWork, IValidatorFactory validatorFactory, AuthorizationRepositories repositories) 
            : base(unitOfWork, validatorFactory)
        {
            repositories.EnsureIsResolved();
            _db = repositories;
        }

        public LoginResultModel Login(LoginCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var user = _db.Users.QueryOne(command.ToDbQuery());
                if (user != null)
                {
                    var failure = VerifyAccess(user);
                    if (failure != null)
                    {
                        return failure;
                    }
                    if (Password.Validate(user.PasswordHash, user.PasswordSalt, command.Password))
                        return new LoginSuccessModel(new UserMessage(Messages.SuccessLogin), user.ToModel<User, UserIdentityModel>());
                    ProcessFailedLogin(user);
                    UnitOfWork.Commit();
                }
                return new LoginFailureModel(LoginFailureReason.BadCredentials.UserMessage(), LoginFailureReason.BadCredentials);
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

        public LoginResultModel RefreshLogin(IdentityQuery<Guid> query)
        {
            EnsureIsValid(query);
            try
            {
                var user = _db.Users.QueryIdentity(query);
                if (user == null)
                {
                    return new LoginFailureModel(new UserMessage(Messages.InvalidCredentials), LoginFailureReason.BadCredentials);
                }
                var failure = VerifyAccess(user);
                if (failure != null)
                {
                    return failure;
                }
                return new LoginSuccessModel(new UserMessage(Messages.SuccessLogin), user.ToModel<User, UserIdentityModel>());
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get user.", ex);
            }
        }

        private static LoginFailureModel VerifyAccess(User user)
        {
            if (user.Deleted)
                return new LoginFailureModel(LoginFailureReason.UserDeleted.UserMessage(), LoginFailureReason.UserDeleted);
                
            if (user.LockoutEnabled)
                return new LoginFailureModel(LoginFailureReason.UserBlocked.UserMessage(), LoginFailureReason.UserBlocked);
                
            return null;
        }

        private static void ProcessFailedLogin(User user)
        {
            ++user.AccessFailedCount;
            if (user.AccessFailedCount % 3 == 0)
            {
                user.LockoutEnabled = true;
                user.LockoutEndDateUtc = DateTime.MaxValue;
            }
        }
    }
}
