using System;
using FluentValidation;
using FluentValidation.Results;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Services.Common;
using VaBank.Services.Common.Security;
using VaBank.Services.Contracts.Processing.Commands;

namespace VaBank.Services.Processing
{
    internal class CardSecurityValidator : SecurityValidator<ICardWithdrawalCommand>
    {
        private readonly IRepository<UserCard> _userCards; 

        public CardSecurityValidator(VaBankIdentity identity, 
            IRepository<UserCard> userCardRepository) : base(identity)
        {
            Argument.NotNull(userCardRepository, "userCardRepository");
            _userCards = userCardRepository;
            Inherit(new AuthenticatedSecurityValidator(identity));
            Custom(NotBlocked);
            Custom(NotExpired);
        }

        private ValidationFailure NotExpired(ICardWithdrawalCommand command)
        {
            var card = _userCards.SurelyFind(command.FromCardId);
            return card.IsExpired ? new ValidationFailure(RootPropertyName, Messages.CardExpired) : null;
        }

        private ValidationFailure NotBlocked(ICardWithdrawalCommand command)
        {
            var card = _userCards.SurelyFind(command.FromCardId);
            return card.Settings.Blocked ? new ValidationFailure(RootPropertyName, Messages.CardBlocked) : null;
        }
    }

    internal class PersonalTransferCommandValidator : AbstractValidator<PersonalCardTransferCommand>
    {
        public PersonalTransferCommandValidator()
        {
            RuleFor(x => x.FromCardId).NotEqual(Guid.Empty);
            RuleFor(x => x.ToCardId).NotEqual(Guid.Empty);
            //TODO: add minimum amounts
            RuleFor(x => x.Amount);
        }
    }

    internal class InterbankTransferCommandValidator : AbstractValidator<InterbankCardTransferCommand>
    {
        private readonly IQueryRepository<UserCard> _userCards;

        public InterbankTransferCommandValidator(IQueryRepository<UserCard> userCards)
        {
            Argument.NotNull(userCards, "cards");
            _userCards = userCards;
            RuleFor(x => x.FromCardId).NotEqual(Guid.Empty);
            RuleFor(x => x.ToCardNo).NotEmpty();
            RuleFor(x => x.ToCardExpirationDateUtc).GreaterThan(x => DateTime.UtcNow);

            //TODO: add minimum amounts
            //RuleFor(x => x.Amount).GreaterThan()
            RuleFor(x => x.ToCardNo)
                .Must(DestinationCardExists)
                .WithLocalizedMessage(() => Messages.DestinationCardNotFound);
        }

        private bool DestinationCardExists(InterbankCardTransferCommand command, string cardNo)
        {
            var card = _userCards.QueryOne(DbQuery.For<UserCard>()
                .FilterBy(UserCard.Spec.ByCardNumberAndExpiration(command.ToCardNo, command.ToCardExpirationDateUtc)));
            return card != null;
        }
    }
}
