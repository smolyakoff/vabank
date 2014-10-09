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
    public class MembershipService : BaseService, IMembershipService
    {
        private readonly MembershipRepositories _db;

        public MembershipService(IUnitOfWork unitOfWork, IValidatorFactory validatorFactory, MembershipRepositories repositories) 
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
                    //TODO: refactor messages based on failure reason (might be extension method)
                    //TODO: check access failed count (business rule AM001.3)
                    if (user.Deleted)
                        return new LoginFailureModel(new UserMessage(Messages.UserDeleted), LoginFailureReason.UserDeleted);
                    if (user.LockoutEnabled)
                        return new LoginFailureModel(new UserMessage(Messages.UserBlocked), LoginFailureReason.UserBlocked);
                    if (Password.Validate(user.PasswordHash, user.PasswordSalt, command.Password))
                        return new LoginSuccessModel(new UserMessage(Messages.SuccessLogin), user.ToModel<User, UserIdentityModel>());
                    ++user.AccessFailedCount;
                    UnitOfWork.Commit();
                }
                return new LoginFailureModel(new UserMessage(Messages.InvalidCredentials), LoginFailureReason.BadCredentials);
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
                return command;
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

        public UserIdentityModel GetUser(IdentityQuery<Guid> query)
        {
            EnsureIsValid(query);
            try
            {
                var user = _db.Users.ProjectIdentity<Guid, User, UserIdentityModel>(query);
                return user;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get user.", ex);
            }
        }
    }
}
