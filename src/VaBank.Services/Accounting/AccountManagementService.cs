using System.Security.Cryptography.X509Certificates;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Core.Accounting.Entities;
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
        private readonly AccountingDependencies _deps;

        public AccountManagementService(BaseServiceDependencies dependencies, AccountingDependencies accountingDependencies)
            :base(dependencies)
        {
            dependencies.EnsureIsResolved();
            _deps = accountingDependencies;
        }

        public AccountingLookupModel GetAccountingLookup()
        {
            try
            {
                var lookup = new AccountingLookupModel
                {
                    CardVendors = _deps.CardVendors.ProjectAll<CardVendorModel>().ToList(),
                    Currencies = _deps.Currencies.ProjectAll<CurrencyModel>().ToList()
                };
                return lookup;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get accounting lookup.", ex);
            }
        }

        public IList<UserCardModel> GetUserCards(IdentityQuery<Guid> userId)
        {
            try
            {
                var userCards = _deps.UserCards.Project<UserCardModel>(userId.ToDbQuery<UserCard>());
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
