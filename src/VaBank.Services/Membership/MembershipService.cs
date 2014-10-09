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
                    if (user.Deleted)
                        return new LoginFailureModel(new UserMessage(Messages.UserDeleted), LoginFailureReason.UserDeleted);
                    if (user.LockoutEnabled)
                        return new LoginFailureModel(new UserMessage(Messages.UserBlocked), LoginFailureReason.UserBlocked);
                    if (Password.Validate(user.PasswordHash, user.PasswordSalt, command.Password))
                        return new LoginSuccessModel(new UserMessage(Messages.SuccessLogin));
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
                _db.ApplicationTokens.Create(token);
                UnitOfWork.Commit();
                return command;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't create application token.", ex);
            }
        }

        public TokenModel GetToken(IdentityQuery<string> query)
        {
            EnsureIsValid(query);
            try
            {
                var token = _db.ApplicationTokens.ProjectIdentity<string, ApplicationToken, TokenModel>(query);
                return token;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get application token.", ex);
            }
        }

        public bool RemoveToken(IdentityQuery<string> query)
        {
            EnsureIsValid(query);
            try
            {
                var token = _db.ApplicationTokens.QueryIdentity(query);
                if (token != null)
                {
                    _db.ApplicationTokens.Delete(token);
                    UnitOfWork.Commit();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't remove application token.", ex);
            }
        }

        public ApplicationClientModel GetClient(IdentityQuery<string> query)
        {
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
    }
}
