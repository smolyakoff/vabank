using FluentValidation;
using System;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Accounting.Factories;
using VaBank.Services.Contracts.Processing.Commands;

namespace VaBank.Services.Processing
{
    internal class CardCommandValidator : AbstractValidator<ICardCommand>
    {
        private readonly IRepository<UserCard> _userCards;
        private readonly CardLimitsFactory _cardLimitsFactory;

        public CardCommandValidator(IRepository<UserCard> userCards,
            CardLimitsFactory cardLimitsFactory)
        {
            Argument.NotNull(userCards, "userCards");
            Argument.NotNull(cardLimitsFactory, "cardLimitsFactory");
            _userCards = userCards;
            _cardLimitsFactory = cardLimitsFactory;
            
            //TODO: add validation rules
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.FromCardId).Must(CardExists)
                .WithLocalizedMessage(() => Messages.CardDoesNotExistError);
            RuleFor(x => x.FromCardId).Must(CardIsNotBlocked)
                .WithLocalizedMessage(() => Messages.CardIsBlockedError);
            RuleFor(x => x.FromCardId).Must(CardIsNotExpired)
                .WithLocalizedMessage(() => Messages.CardIsInvalidError);
        }

        private bool CardExists(Guid cardId)
        {
            var card = _userCards.Find(cardId);
            return card != null;
        }

        //TODO: move to security validator
        private bool CardIsNotBlocked(Guid cardId)
        {
            var card = _userCards.Find(cardId);
            if (card != null)
            {
                return !card.Settings.Blocked;
            }
            return true;
        }

        private bool CardIsNotExpired(Guid cardId)
        {
            //TODO: check month only
            var card = _userCards.Find(cardId);
            if (card != null)
            {
                return card.ExpirationDateUtc > DateTime.UtcNow;
            }
            return true;
        }

        private bool AmmountSatisfiesLimits(ICardCommand command, decimal amount)
        {
            throw new NotImplementedException();        
        }
    }
}
