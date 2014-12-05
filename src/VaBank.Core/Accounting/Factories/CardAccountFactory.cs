using System;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Util;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Membership.Entities;
using VaBank.Core.Processing;

namespace VaBank.Core.Accounting.Factories
{
    [Injectable]
    public class CardAccountFactory
    {
        private readonly IRepository<CardAccount> _cardAccountRepository;
        private readonly IRepository<Bank> _bankRepository;

        private readonly BankSettings _bankSettings;

        private const string IndividualAccountPrefix = "3014";

        public CardAccountFactory(IRepository<CardAccount> cardAccountRepository, 
            IRepository<Bank> bankRepository)
        {
            Argument.NotNull(cardAccountRepository, "cardAccountRepository");
            Argument.NotNull(bankRepository, "bankRepository");

            _bankRepository = bankRepository;
            _cardAccountRepository = cardAccountRepository;

            _bankSettings = new BankSettings();
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
            
            var bank = _bankRepository.Find(_bankSettings.VaBankCode);
            if (bank == null)
                throw new InvalidOperationException("VaBank bank doesn't exist at the database. Please, check VaBank bank code or create it.");
            
            var account = new CardAccount(accountNo, currency, owner, bank)
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