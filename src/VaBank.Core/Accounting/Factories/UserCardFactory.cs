using System;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Accounting.Factories
{
    [Injectable]
    public class UserCardFactory
    {
        private readonly CardFactory _cardFactory;

        private readonly CardLimitsFactory _cardLimitsFactory;


        public UserCardFactory(CardFactory cardFactory, CardLimitsFactory cardLimitsFactory)
        {
            Argument.NotNull(cardFactory, "cardFactory");
            Argument.NotNull(cardLimitsFactory, "cardLimitsFactory");

            _cardFactory = cardFactory;
            _cardLimitsFactory = cardLimitsFactory;
        }

        public UserCard Create(
            CardAccount cardAccount,
            CardVendor cardVendor,
            User cardOwner,
            string cardholderFirstName,
            string cardholderLastName,
            DateTime expirationDateUtc)
        {
            Argument.NotNull(cardAccount, "cardAccount");
            Argument.NotNull(cardVendor, "cardVendor");
            Argument.NotNull(cardOwner, "cardOwner");

            var card = _cardFactory.Create(
                cardVendor,
                cardholderFirstName,
                cardholderLastName,
                expirationDateUtc);
            var settings = new CardSettings(card.Id, _cardLimitsFactory.CreateDefault(cardAccount.Currency));
            var userCard = new UserCard(cardAccount, card, cardOwner, settings);
            return userCard;
        }
    }
}
