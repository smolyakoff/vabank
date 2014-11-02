using AutoMapper;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Core.Accounting.Entities;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Commands;
using VaBank.Services.Contracts.Accounting.Events;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Accounting.Queries;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Accounting
{
    public class CardAccountService : BaseService, ICardAccountService
    {
        private readonly AccountingDependencies _deps;

        public CardAccountService(BaseServiceDependencies dependencies,
            AccountingDependencies accountingDependencies)
            : base(dependencies)
        {
            dependencies.EnsureIsResolved();
            _deps = accountingDependencies;
        }

        public CardLookupModel GetAccountingLookup()
        {
            try
            {
                var lookup = new CardLookupModel
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

        public IList<CustomerCardModel> GetCustomerCards(IdentityQuery<Guid> userId)
        {
            EnsureIsValid(userId);
            try
            {
                var cards = _deps.UserCards.PartialQuery(
                    UserCard.Spec.Linked,
                    userId.ToPropertyQuery<UserCard, Guid>(x => x.Owner.Id));
                return cards.Map<UserCard, CustomerCardModel>().ToList();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get customer cards.", ex);
            }
        }

        public IPagedList<CardAccountBriefModel> GetCardAccounts(AccountQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var accounts =
                    _deps.CardAccounts.ProjectThenQueryPage<CardAccountBriefModel>(
                        query.ToDbQuery<CardAccountBriefModel>());
                return accounts;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot get card accounts.", ex);
            }
        }

        public IList<UserCardModel> GetUserCards(CardQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var dbQuery = query.ToDbQuery<UserCardModel>();
                var ownedCards = _deps.UserCards.ProjectThenQuery<UserCardModel>(dbQuery);
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

        public IList<CustomerCardModel> GetUserCards(IdentityQuery<Guid> userId)
        {
            EnsureIsValid(userId);
            try
            {
                var userCards = _deps.UserCards.Project<CustomerCardModel>(userId.ToDbQuery<UserCard>());
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
            catch (ServiceException)
            {
                throw;
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
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot create card account.", ex);
            }
        }

        public UserMessage SetCardBlock(SetCardBlockCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var userCard = _deps.UserCards.SurelyFind(command.CardId);
                var cardName = userCard.Settings.FriendlyName ?? userCard.CardVendor.Name;
                UserMessage message;
                var events = new List<ApplicationEvent>();
                if (command.Blocked)
                {
                    message = UserMessage.ResourceFormat(() => Messages.CardBlocked, cardName);
                    userCard.Block();
                    events.Add(new UserCardBlocked(userCard.ToModel<UserCard, CustomerCardModel>(), Operation.Id));
                }
                else
                {
                    message = UserMessage.ResourceFormat(() => Messages.CardUnblocked, cardName);
                    userCard.Unblock();
                }
                Commit();
                events.ForEach(Publish);
                return message;
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot set card block.", ex);
            }
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
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot set card assignment.", ex);
            }
        }

        public UserMessage UpdateCardSettings(UpdateCardSettingsCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var events = new List<ApplicationEvent>();
                var userCard = _deps.UserCards.SurelyFind(command.CardId);
                if (!string.IsNullOrEmpty(command.FriendlyName))
                {
                    userCard.Settings.FriendlyName = command.FriendlyName;                    
                }
                if (command.CardLimits != null)
                {
                    Mapper.Map(command.CardLimits, userCard.Settings.Limits);
                    events.Add(new UserCardLimitChanged(userCard.ToModel<UserCard, CustomerCardModel>(), Operation.Id));
                }
                Commit();
                events.ForEach(Publish);
                return UserMessage.Resource(() => Messages.CardSettingsUpdated);
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Cannot update card settings.", ex);
            }
        }
    }
}