using System;
using System.Collections.Generic;
using System.Linq;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Util;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Accounting.Factories
{
    [Injectable]
    public class CardFactory
    {
        private readonly IQueryRepository<Card> _cardRepository;

        private readonly Dictionary<string, Func<string>> _prefixes = new Dictionary<string, Func<string>>
        {
            {"visa", () => "4"},
            {"mastercard", () => "5"},
            {"maestro", () => Randomizer.Choose("3", "6")}
        };

        private const string VabankCardCode = "666";

        private const string EmitentCode = "00";

        public CardFactory(IQueryRepository<Card> cardRepository)
        {
            Argument.NotNull(cardRepository, "cardRepository");
            _cardRepository = cardRepository;
        }

        public Card Create(
            CardVendor cardVendor,
            string cardholderFirstName,
            string cardholderLastName,
            DateTime expirationDateTimeUtc)
        {
            Argument.NotNull(cardVendor, "cardVendor");
            Argument.NotEmpty(cardholderFirstName, "cardholderFirstName");
            Argument.NotNull(cardholderLastName, "cardholderLastName");
            Argument.EnsureIsValid<CardExpirationDateValidator, DateTime>(expirationDateTimeUtc, "expirationDateTimeUtc");
            if (!_prefixes.ContainsKey(cardVendor.Id))
            {
                var message = string.Format("Card vendor [{0}] is not supported.", cardVendor.Id);
                throw new NotSupportedException(message);
            }

            string cardNo;
            while (true)
            {
                cardNo = GenerateCardNo(cardVendor);
                var number = cardNo;
                var query = DbQuery.For<Card>().FilterBy(x => x.CardNo == number);
                var existingCard =  _cardRepository.Query(query).FirstOrDefault();
                if (existingCard == null)
                {
                    break;
                }
            }
            var card = new Card(cardNo, cardVendor, cardholderFirstName.ToUpper(), cardholderLastName.ToUpper(), expirationDateTimeUtc);
            return card;
        }

        private string GenerateCardNo(CardVendor cardVendor)
        {
            var cardNo = string.Format("{0}{1}{2}{3}{4}",
                _prefixes[cardVendor.Id](),
                VabankCardCode,
                Randomizer.NumericString(2),
                EmitentCode,
                Randomizer.NumericString(8));
            return cardNo;
        }
    }
}
