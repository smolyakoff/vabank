using System;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Util;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Accounting.Factories
{
    [Injectable]
    public class CardAccountFactory
    {
        private readonly IRepository<CardAccount> _cardAccountRepository;        

        private const string IndividualAccountPrefix = "3014";

        public CardAccountFactory(IRepository<CardAccount> cardAccountRepository)
        {
            Argument.NotNull(cardAccountRepository, "cardAccountRepository");

            _cardAccountRepository = cardAccountRepository;
        }

        public CardAccount Create(Currency currency, User owner, decimal initalBalance, DateTime expirationDateUtc)
        {
            Argument.NotNull(currency, "currency");
            Argument.NotNull(owner, "owner");

            string accountNo;
            while (true)
            {
                accountNo = GenerateAccountNo();
                var existingAccount = _cardAccountRepository.Find(accountNo);
                if (existingAccount == null)
                {
                    break;
                }
            }
                        
            var account = new CardAccount(accountNo, currency, owner)
            {
                ExpirationDateUtc = expirationDateUtc
            };
            account.Deposit(initalBalance);
            return account;
        }

        private static string GenerateAccountNo()
        {
            return string.Format("{0}{1}", IndividualAccountPrefix, Randomizer.NumericString(9));
        }
    }
}