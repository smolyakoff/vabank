using AutoMapper;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Entities;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Accounting;
using VaBank.Services.Contracts.Accounting.Commands;
using VaBank.Services.Contracts.Accounting.Events;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Accounting.Queries;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Processing.Events;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Accounting
{
    public class CardAccountService : BaseService, ICardAccountService
    {
        private readonly AccountingDependencies _deps;

        private readonly BankSettings _bankSettings = new BankSettings();

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
                    UserCard.Spec.Active,
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
                var accounts = _deps.CardAccounts.QueryPage(query.ToDbQuery<CardAccount>());
                return accounts.Map<CardAccountBriefModel>();
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


        public IList<CardModel> GetAccountCards(AccountCardsQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var userCards = _deps.UserCards.Project<CardModel>(query.ToDbQuery());
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
                var name = _deps.TransactionReferenceBook.ForCashDeposit();
                var money = new Money(currency, command.InitialBalance);
                var deposit = cardAccount.Deposit(userCard, name.Code, name.Description, _bankSettings.Location, money, _deps.MoneyConverter);
                _deps.Transactions.Create(deposit);
                Commit();
                Publish(new TransactionProgressEvent(Operation.Id, deposit.ToModel<TransactionModel>()));
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

        public UserMessage SetCardActivation(SetCardActivationCommand command)
        {
            EnsureIsValid(command);
            try
            {
                var userCard = _deps.UserCards.SurelyFind(command.CardId);
                var message = command.IsActive
                    ? UserMessage.Resource(() => Messages.CardActivated)
                    : UserMessage.Resource(() => Messages.CardDeactivated);
                userCard.IsActive = command.IsActive;
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

        public CardAccountBriefModel GetCardAccountBrief(IdentityQuery<string> accountNo)
        {
            EnsureIsValid(accountNo);
            try
            {
                var query = DbQuery.For<CardAccount>().FilterBy(x => x.AccountNo == accountNo.Id);
                var account = _deps.CardAccounts.QueryOne(query);
                return account == null ? null : account.ToModel<CardAccountBriefModel>();
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get card account brief information.", ex);
            }
        }

        public CardAccountStatementModel GetCardAccountStatement(CardAccountStatementQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var cardAccount = _deps.CardAccounts.SurelyFind(query.AccountNo);
                var transactions = _deps.Transactions.Query(query.ToDbQuery(cardAccount));
                var statement = new CardAccountStatementModel
                {
                    AccountCurrency = cardAccount.Currency.ToModel<CurrencyModel>(),
                    AccountNo = cardAccount.AccountNo,
                    AccountOwner = cardAccount.Owner.ToModel<UserNameModel>(),
                    CreatedDateUtc = DateTime.UtcNow,
                    DateRange = query.DateRange,
                    StatementBalance = transactions.AsQueryable()
                        .Where(Specs.ForTransaction.CalculatedDeposits || Specs.ForTransaction.CalculatedWithdrawals)
                        .Sum(x => x.AccountAmount),
                    StatementDeposits = transactions.AsQueryable()
                        .Where(Specs.ForTransaction.CalculatedDeposits)
                        .Sum(x => x.AccountAmount),
                    StatementWithdrawals = transactions.AsQueryable()
                        .Where(Specs.ForTransaction.CalculatedWithdrawals)
                        .Sum(x => x.AccountAmount),
                    Transactions = transactions.Map<CardAccountStatementItemModel>().ToList()
                };
                return statement;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get card account statement.", ex);
            }
        }

        public AccountBalanceModel GetCardBalance(CardBalanceQuery query)
        {
            EnsureIsValid(query);
            try
            {
                var account = _deps.UserCards.SurelyFind(query.Id).Account;
                if (account == null)
                {
                    throw new InvalidOperationException("Card should be bound to the account.");
                }
                var currency = string.IsNullOrEmpty(query.CurrencyISOName)
                    ? account.Currency
                    : _deps.Currencies.Find(query.CurrencyISOName);
                var balanceMoney = new Money(account.Currency, account.Balance);
                var balance = new AccountBalanceModel
                {
                    AccountNo = account.AccountNo,
                    AccountBalance = account.Balance,
                    AccountCurrency = account.Currency.ToModel<CurrencyModel>(),
                    RequestedCurrency = currency.ToModel<CurrencyModel>(),
                    RequestedBalance = _deps.MoneyConverter.Convert(balanceMoney, currency.ISOName).Amount
                };
                return balance;
            }
            catch (ServiceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't get account balance.", ex);
            }
        }
    }
}