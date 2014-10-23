using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Commands;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Accounting.Queries;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Common.Queries;

namespace VaBank.Services.Accounting
{
    public class AccountManagementService : BaseService, IAccountManagementService
    {
        private readonly AccountingRepositories _db;

        public AccountManagementService(BaseServiceDependencies dependencies, AccountingRepositories repositories)
            :base(dependencies)
        {
            repositories.EnsureIsResolved();
            _db = repositories;
        }

        public AccountingLookupModel GetLookup()
        {
            try
            {
                var currencies = _db.Currencies.ProjectAll<CurrencyModel>();
                return new AccountingLookupModel() { Currencies = currencies.ToList() };
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot get accounting lookup.", ex);
            }
        }

        public IList<UserCardModel> GetUserCards(IdentityQuery<Guid> userId)
        {
            try
            {
                var userCards = _db.UserCards.ProjectThenQuery<UserCardModel>(userId);
                return userCards;
            }
            catch(Exception ex)
            {
                throw new ServiceException("Cannot get user cards");
            }
        }

        public IPagedList<CardAccountBriefModel> GetCardAccounts(CardAccountsQuery query)
        {
            try
            {
                //var cardAccounts = _db.CardAccounts.ProjectThenQueryPage<CardAccountModel>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot get card accounts", ex);
            }
        }

        public RequestedCardAccountModel RequestCardAccount(IdentityQuery<Guid> userId)
        {
            throw new NotImplementedException();
        }

        public UserMessage CreateCardAccount(CreateCardAccountCommand command)
        {
            throw new NotImplementedException();
        }

        public UserMessage SetCardBlock(SetCardBlockCommand command)
        {
            throw new NotImplementedException();
        }

        public UserMessage SetCardAssignment(SetCardAssignmentCommand command)
        {
            throw new NotImplementedException();
        }

        public UserMessage SetCardLimits(SetCardLimitsCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
