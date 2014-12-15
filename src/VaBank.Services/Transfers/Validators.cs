using System;
using FluentValidation;
using FluentValidation.Validators;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Processing.Commands;
using VaBank.Services.Contracts.Transfers.Commands;
using VaBank.Services.Processing;

namespace VaBank.Services.Transfers
{
    internal class CardTransferCommandValidator<TCommand> : AbstractValidator<TCommand>
    where TCommand : ICardWithdrawalCommand
    {
        protected readonly CardTransferSettings _settings;
        protected readonly IRepository<UserCard> _userCardRepository;

        public CardTransferCommandValidator(CardTransferSettings settings, IRepository<UserCard> userCardRepository)
        {
            _settings = settings;
            _userCardRepository = userCardRepository;
            Argument.NotNull(settings, "settings");
            Argument.NotNull(userCardRepository, "userCardRepository");

            RuleFor(x => x.FromCardId).NotEqual(Guid.Empty);
            RuleFor(x => x.Amount)
                .Must(GreaterThanOrEqualToMinimumAmount).WithLocalizedMessage(() => Messages.CardTransferSmallAmount)
                .Must(LessThanOrEqualToAccountBalance).WithLocalizedMessage(() => Messages.InsufficientFunds);
        }

        private bool LessThanOrEqualToAccountBalance(TCommand command, decimal amount)
        {
            var userCard = _userCardRepository.SurelyFind(command.FromCardId);
            if (userCard.Account == null)
            {
                throw new InvalidOperationException("User card is not bound to the account.");
            }
            return userCard.Account.Balance >= amount;
        }

        private bool GreaterThanOrEqualToMinimumAmount(TCommand command, decimal amount, PropertyValidatorContext context)
        {
            var userCard = _userCardRepository.SurelyFind(command.FromCardId);
            if (userCard.Account == null)
            {
                throw new InvalidOperationException("User card is not bound to the account.");
            }
            var minimal = _settings.MinimalAmounts.ContainsKey(userCard.Account.Currency.ISOName)
                ? _settings.MinimalAmounts[userCard.Account.Currency.ISOName]
                : 0;
            if (amount < minimal)
            {
                context.MessageFormatter.AppendArgument("MinimalAmount", minimal);
                context.MessageFormatter.AppendArgument("CurrencyISOName", userCard.Account.Currency.ISOName);
                return false;
            }
            return true;
        }
    }

    internal class PersonalTransferCommandValidator : CardTransferCommandValidator<PersonalCardTransferCommand>
    {
        public PersonalTransferCommandValidator(
            CardTransferSettings settings,
            IRepository<UserCard> userCardRepository) :
            base(settings, userCardRepository)
        {
            RuleFor(x => x.ToCardId).NotEqual(Guid.Empty).NotEqual(x => x.FromCardId);
        }
    }

    internal class InterbankTransferCommandValidator : CardTransferCommandValidator<InterbankCardTransferCommand>
    {
        private readonly IQueryRepository<UserCard> _userCards;

        public InterbankTransferCommandValidator(CardTransferSettings settings,
            IRepository<UserCard> userCardRepository,
            IQueryRepository<UserCard> userCards)
            : base(settings, userCardRepository)
        {
            Argument.NotNull(userCards, "userCards");
            _userCards = userCards;

            RuleFor(x => x.ToCardNo).NotEmpty();
            RuleFor(x => x.ToCardExpirationDateUtc).GreaterThan(x => DateTime.UtcNow);
            RuleFor(x => x.ToCardNo)
                .Must(DestinationCardExists)
                .WithLocalizedMessage(() => Messages.DestinationCardNotFound)
                .Must(NotEqualToFromCardNo)
                .WithLocalizedMessage(() => Messages.DestinationCardNotEqualToSource);
        }

        private bool NotEqualToFromCardNo(InterbankCardTransferCommand command, string toCardNo)
        {
            var fromCard = _userCards.Find(command.FromCardId);
            return fromCard.CardNo != toCardNo;
        }

        private bool DestinationCardExists(InterbankCardTransferCommand command, string cardNo)
        {
            var card = _userCards.QueryOne(DbQuery.For<UserCard>()
                .FilterBy(UserCard.Spec.ByCardNumberAndExpiration(command.ToCardNo, command.ToCardExpirationDateUtc)));
            return card != null;
        }
    }
}
