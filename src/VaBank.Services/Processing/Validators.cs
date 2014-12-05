using FluentValidation.Results;
using VaBank.Common.Data.Repositories;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.App.Entities;
using VaBank.Services.Common;
using VaBank.Services.Common.Security;
using VaBank.Services.Contracts.Processing.Commands;

namespace VaBank.Services.Processing
{
    internal class CardSecurityValidator : SecurityValidator<ICardWithdrawalCommand>
    {
        private readonly IRepository<UserCard> _userCards; 

        public CardSecurityValidator(VaBankIdentity identity, 
            IRepository<UserCard> userCardRepository, IRepository<SecurityCode> securityCodeRepository) : base(identity)
        {
            Argument.NotNull(userCardRepository, "userCardRepository");
            Argument.NotNull(securityCodeRepository, "securityCodeRepository");

            _userCards = userCardRepository;
            Inherit(new CodeSecurityValidator(identity, securityCodeRepository));
            Custom(UserOwnsCard);
            Custom(NotBlocked);
            Custom(NotExpired);
        }

        public ValidationFailure UserOwnsCard(ICardWithdrawalCommand command)
        {
            var card = _userCards.SurelyFind(command.FromCardId);
            return card.Owner.Id == Identity.UserId
                ? null
                : new ValidationFailure(RootPropertyName, Messages.CardAccessDenied);
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
}
