using System;
using Newtonsoft.Json.Linq;
using VaBank.Common.Data;
using VaBank.Common.Data.Repositories;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Payments.Entities;
using VaBank.Core.Processing;
using VaBank.Core.Processing.Resources;

namespace VaBank.Core.Payments.Factories
{
    [Injectable]
    public class CardPaymentFactory
    {
        private readonly MoneyConverter _moneyConverter;

        private readonly TransactionReferenceBook _transactionReferenceBook;

        private readonly BankSettings _settings;

        private readonly PaymentFormFactory _paymentFormFactory;

        private readonly IRepository<UserPaymentProfile> _paymentProfiles;

        private readonly IRepository<Account> _accounts;

        private readonly IRepository<Currency> _currencies;

        private readonly IQueryRepository<CorrespondentAccount> _correspondentAccounts; 

        public CardPaymentFactory(
            IRepository<Account> accounts, 
            IRepository<UserPaymentProfile> paymentProfiles,
            IRepository<Currency> currencies,
            IQueryRepository<CorrespondentAccount> correspondentAccounts, 
            TransactionReferenceBook transactionReferenceBook,
            MoneyConverter moneyConverter,
            PaymentFormFactory paymentFormFactory)
        {
            Argument.NotNull(accounts, "accounts");
            Argument.NotNull(paymentProfiles, "paymentProfiles");
            Argument.NotNull(currencies, "currencies");
            Argument.NotNull(correspondentAccounts, "correspondentAccounts");
            Argument.NotNull(transactionReferenceBook, "transactionReferenceBook");
            Argument.NotNull(moneyConverter, "moneyConverter");
            Argument.NotNull(paymentFormFactory, "paymentFormFactory");

            _settings = new BankSettings();
            _currencies = currencies;
            _correspondentAccounts = correspondentAccounts;
            _paymentProfiles = paymentProfiles;
            _accounts = accounts;
            _transactionReferenceBook = transactionReferenceBook;
            _moneyConverter = moneyConverter;
            _paymentFormFactory = paymentFormFactory;
        }

        public CardPayment Create(UserCard card, PaymentTemplate template, JObject form)
        {
            Argument.NotNull(template, "template");
            Argument.NotNull(form, "form");
            Argument.NotNull(card, "card");

            var paymentProfile = _paymentProfiles.Find(card.Owner.Id);
            if (paymentProfile == null)
            {
                var message = string.Format("Payment profile for user [{0}] was not found.", card.Owner.Id);
                throw new InvalidOperationException(message);
            }
            var paymentForm = _paymentFormFactory.Create(paymentProfile, card.Account, template);
            paymentForm.MergeWith(form);
            var paymentOrder = template.OrderTemplate.CreateOrder(paymentForm);
            var to = paymentOrder.BeneficiaryBankCode == _settings.VaBankCode
                ? _accounts.Find(paymentOrder.BeneficiaryAccountNo)
                : _correspondentAccounts.QueryOne(DbQuery.For<CorrespondentAccount>().FilterBy(x => x.Bank.Code == paymentOrder.BeneficiaryBankCode));
            if (to == null)
            {
                var message = string.Format("Destination account could not be found. Bank code: {0}. Account No: {1}.",
                    paymentOrder.BeneficiaryBankCode,
                    paymentOrder.BeneficiaryAccountNo);
                throw new InvalidOperationException(message);
            }
            var currency = _currencies.Find(paymentOrder.CurrencyISOName);
            if (currency == null)
            {
                var message = string.Format("Currency with code {0} was not found.", paymentOrder.CurrencyISOName);
                throw new InvalidOperationException(message);
            }
            var payment = new CardPayment(card, template, paymentOrder, form, card.Account, to, currency);
            var transactionName = _transactionReferenceBook.ForOperation(payment);
            payment.Withdrawal = card.Account.Withdraw(
                card,
                transactionName.Code,
                transactionName.Description,
                _settings.Location,
                payment.MoneyAmount,
                _moneyConverter);
            
            return payment;
        }
    }
}
