using System.Security.Cryptography.X509Certificates;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaBank.Common.Data;
using VaBank.Core.Accounting;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Commands;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Accounting.Queries;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Models;

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
                //TODO: better pass everything through the constructor
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
                //TODO: bad entity design here. Why user card has its own id?
                var userCards = _db.UserCards.Project<UserCardModel>(userId.ToDbQuery<UserCard>());
                return userCards;
            }
            catch(Exception ex)
            {
                throw new ServiceException("Cannot get user cards.", ex);
            }
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

        public AccountingLookupModel GetAccountingLookup()
        {
            throw new NotImplementedException();
        }

        public IPagedList<AccountBriefModel> GetCardAccounts(AccountQuery query)
        {
            throw new NotImplementedException();
        }

        public IList<CardModel> GetCards(IdentityQuery<string> accountNo)
        {
            throw new NotImplementedException();
        }

        public UserMessage CreateCard(CreateCardCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
