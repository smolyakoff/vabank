using System;
using FluentValidation;
using FluentValidation.Results;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Util;
using VaBank.Common.Validation;
using VaBank.Core.Accounting;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Accounting.Factories;
using VaBank.Services.Contracts.Accounting.Commands;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Accounting.Queries;

namespace VaBank.Services.Accounting
{
    internal class CreateCardCommandValidator : AbstractValidator<CreateCardCommand>
    {
        private readonly IRepository<CardAccount> _cardAccountRepository;

        public CreateCardCommandValidator(IRepository<CardAccount> cardAccountRepository)
        {
            _cardAccountRepository = cardAccountRepository;

            RuleFor(x => x.AccountNo)
                .NotEmpty()
                .Must(CardAccountExists);
            RuleFor(x => x.CardVendorId)
                .NotEmpty();
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty);
            RuleFor(x => x.CardholderFirstName)
                .NotEmpty();
            RuleFor(x => x.CardholderLastName)
                .NotEmpty();
            RuleFor(x => x.ExpirationDateUtc)
                .UseValidator(new CardExpirationDateValidator())
                .Must(IsExpirationDateValid)
                .WithLocalizedMessage(() => Messages.LessThanAccountExpirationDate);
        }

        private bool IsExpirationDateValid(CreateCardCommand command, DateTime expirationDate)
        {
            var account = _cardAccountRepository.Find(command.AccountNo);
            if (account == null)
            {
                return false;
            }
            return expirationDate <= account.ExpirationDateUtc;
        }

        private bool CardAccountExists(string accountNo)
        {
            return _cardAccountRepository.Find(accountNo) != null;
        }
    }

    [StaticValidator]
    internal class SetCardActivationCommandValidator : AbstractValidator<SetCardActivationCommand>
    {
        public SetCardActivationCommandValidator()
        {
            RuleFor(x => x.CardId).NotEqual(Guid.Empty);
        }
    }

    [StaticValidator]
    internal class AccountCardsQueryValidator : AbstractValidator<AccountCardsQuery>
    {
        public AccountCardsQueryValidator()
        {
            RuleFor(x => x.AccountNo).UseValidator(new AccountNumberValidator());
        }
    }

    [StaticValidator]
    internal class CreateCardAccountCommandValidator : AbstractValidator<CreateCardAccountCommand>
    {
        public CreateCardAccountCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty);
            RuleFor(x => x.CurrencyISOName)
                .NotEmpty();
            RuleFor(x => x.CardVendorId)
                .NotEmpty();
            RuleFor(x => x.InitialBalance)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.AccountExpirationDateUtc)
                .GreaterThan(x => DateTime.UtcNow.Date);
            RuleFor(x => x.CardholderFirstName)
                .NotEmpty();
            RuleFor(x => x.CardholderLastName)
                .NotEmpty();
            RuleFor(x => x.CardExpirationDateUtc)
                .UseValidator(new CardExpirationDateValidator())
                .Must(LessThanAccountExpirationDate)
                .WithLocalizedMessage(() => Messages.LessThanAccountExpirationDate);
        }

        private static bool LessThanAccountExpirationDate(CreateCardAccountCommand command, DateTime cardExpirationDate)
        {
            return cardExpirationDate <= command.AccountExpirationDateUtc;
        }
    }

    internal class UpdateCardSettingsCommandValidator : AbstractValidator<UpdateCardSettingsCommand>
    {
        private readonly IRepository<UserCard> _userCardRepository;

        private readonly CardLimitsFactory _cardLimitsFactory;

        public UpdateCardSettingsCommandValidator(
            CardLimitsFactory cardLimitsFactory, 
            IRepository<UserCard> userCardRepository)
        {
            Assert.NotNull("cardLimitsRangeRepository", cardLimitsFactory);
            Assert.NotNull("userCardRepository", userCardRepository);

            _cardLimitsFactory = cardLimitsFactory;
            _userCardRepository = userCardRepository;

            RuleFor(x => x.CardId).NotEqual(Guid.Empty).Must(CardExists);
        }

        public override ValidationResult Validate(UpdateCardSettingsCommand command)
        {
            var userCard = _userCardRepository.Find(command.CardId);
            var currency = userCard.Account.Currency;
            var limitsRange = _cardLimitsFactory.FindRange(currency.ISOName);
            if (command.CardLimits != null)
            {
                RuleFor(x => x.CardLimits.AmountPerDayLocal).InclusiveBetween(
                    limitsRange.AmountPerDayLocal.LowerBound,
                    limitsRange.AmountPerDayLocal.UpperBound)
                    .When(x => x.CardLimits != null)
                    .WithLocalizedMessage(() => Messages.CardLimitsAmountLocal, command.CardLimits.AmountPerDayLocal);
                RuleFor(x => x.CardLimits.AmountPerDayAbroad).InclusiveBetween(
                    limitsRange.AmountPerDayAbroad.LowerBound,
                    limitsRange.AmountPerDayAbroad.UpperBound)
                    .When(x => x.CardLimits != null)
                    .WithLocalizedMessage(() => Messages.CardLimitsAmountAbroad, command.CardLimits.AmountPerDayAbroad);
                RuleFor(x => x.CardLimits.OperationsPerDayLocal).InclusiveBetween(
                    limitsRange.OperationsPerDayLocal.LowerBound,
                    limitsRange.OperationsPerDayLocal.UpperBound)
                    .When(x => x.CardLimits != null)
                    .WithLocalizedMessage(() => Messages.CardLimitsDaysLocal, command.CardLimits.OperationsPerDayLocal);
                RuleFor(x => x.CardLimits.OperationsPerDayAbroad).InclusiveBetween(
                    limitsRange.OperationsPerDayAbroad.LowerBound,
                    limitsRange.OperationsPerDayAbroad.UpperBound)
                    .When(x => x.CardLimits != null)
                    .WithLocalizedMessage(() => Messages.CardLimitsDaysAbroad, command.CardLimits.OperationsPerDayAbroad);
            }
            return base.Validate(command);
        }

        private bool CardExists(Guid cardId)
        {
            var userCard = _userCardRepository.Find(cardId);
            return userCard != null && userCard.Account != null;
        }
    }

    internal class SetCardBlockCommandValidator : AbstractValidator<SetCardBlockCommand>
    {
        public SetCardBlockCommandValidator()
        {
            RuleFor(x => x.CardId).NotEqual(Guid.Empty);
        }
    }

    internal class CardBalanceQueryValidator : AbstractValidator<CardBalanceQuery>
    {
        public CardBalanceQueryValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
            RuleFor(x => x.CurrencyISOName).Length(3).When(x => x.CurrencyISOName != null);
        }
    }
}
