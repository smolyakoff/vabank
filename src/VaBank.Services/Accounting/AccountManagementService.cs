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

        public IPagedList<CardAccountBriefModel> GetCardAccounts(AccountQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var accounts = _deps.CardAccounts.ProjectThenQueryPage<CardAccountBriefModel>(query.ToDbQuery<CardAccountBriefModel>());
                return accounts;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot get card accounts.", ex);
            }
        }

        public IList<OwnedCardModel> GetOwnedCards(CardQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var dbQuery = query.ToDbQuery<OwnedCardModel>();
                var ownedCards = _deps.UserCards.ProjectThenQuery<OwnedCardModel>(dbQuery);
                return ownedCards;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot get cards for account.", ex);
            }
        }


        public IList<CardModel> GetAccountCards(IdentityQuery<string> accountNo)
        {
            EnsureIsValid(accountNo);
            try
            {
                var userCards = _deps.UserCards.PartialProject<CardModel>(
                    UserCard.Spec.Linked,
                    accountNo.ToDbQuery<UserCard, string>(x => x.Account.AccountNo));
                return userCards;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot get cards for account.", ex);
            }
        }

        public IList<UserCardModel> GetUserCards(IdentityQuery<Guid> userId)
        {
            EnsureIsValid(userId);
            try
            {
                var userCards = _deps.UserCards.Project<UserCardModel>(userId.ToDbQuery<UserCard>());
                return userCards;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot get user cards.", ex);
            }
        }

        public UserMessage CreateCard(CreateCardCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var cardAccount = _deps.CardAccounts.SurelyFind(command.AccountNo);
                var user = _deps.Users.SurelyFind(command.UserId);
                var cardVendor = _deps.CardVendors.SurelyFind(command.CardVendorId);
                var userCard = _deps.UserCardFactory.Create(
                    cardAccount,
                    cardVendor,
                    user,
                    command.CardholderFirstName,
                    command.CardholderLastName,
                    command.ExpirationDateUtc);
                _deps.UserCards.Create(userCard);
                Commit();
                return UserMessage.ResourceFormat(() => Messages.CardEmitted, userCard.CardNo);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot create card.", ex);
            }
        }

        public UserMessage CreateCardAccount(CreateCardAccountCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var user = _deps.Users.SurelyFind(command.UserId);
                var cardVendor = _deps.CardVendors.SurelyFind(command.CardVendorId);
                var currency = _deps.Currencies.SurelyFind(command.CurrencyISOName);
                var cardAccount = _deps.CardAccountFactory.Create(
                    currency, 
                    user, 
                    command.InitialBalance,
                    command.AccountExpirationDateUtc);
                _deps.CardAccounts.Create(cardAccount);
                var userCard = _deps.UserCardFactory.Create(
                    cardAccount,
                    cardVendor,
                    user,
                    command.CardholderFirstName,
                    command.CardholderLastName,
                    command.CardExpirationDateUtc);
                _deps.UserCards.Create(userCard);
                Commit();
                return UserMessage.ResourceFormat(() => Messages.AccountOpened, cardAccount.AccountNo);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot create card account.", ex);
            }
        }

        public UserMessage SetCardBlock(SetCardBlockCommand command)
        {
            throw new NotImplementedException();
        }

        public UserMessage SetCardAssignment(SetCardAssignmentCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var userCard = _deps.UserCards.SurelyFind(command.CardId);
                UserMessage message;
                if (!command.Assigned)
                {
                    userCard.Unlink();
                    message = UserMessage.Resource(() => Messages.CardUnlinked);
                }
                else
                {
                    var cardAccount = _deps.CardAccounts.SurelyFind(command.AccountNo);
                    userCard.LinkTo(cardAccount);
                    message = UserMessage.Resource(() => Messages.CardLinked);
                }
                Commit();
                return message;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot set card assignment.", ex);
            }
        }

        public UserMessage SetCardLimits(SetCardLimitsCommand command)
        {
            throw new NotImplementedException();
        }

       
    }
}
