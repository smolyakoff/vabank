using System.Linq;
using Newtonsoft.Json.Linq;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Membership.Entities;
using VaBank.Core.Payments.Entities;
using VaBank.Core.Processing;

namespace VaBank.Core.Payments.Factories
{
    [Injectable(Lifetime = Lifetime.Singleton)]
    public class PaymentFormFactory
    {
        private readonly BankSettings _bankSettings;

        public PaymentFormFactory()
        {
            _bankSettings = new BankSettings();
        }

        public PaymentForm Create(UserPaymentProfile profile, Account account, PaymentTemplate template)
        {
            Argument.NotNull(profile, "profile");
            Argument.NotNull(account, "account");
            Argument.NotNull(template, "template");

            var form = new JObject
            {
                {"payerTin", new JValue(profile.PayerTIN)},
                {"payerName", new JValue(profile.PayerName)},
                {"payerBankCode", new JValue(_bankSettings.VaBankCode)},
                {"payerAccountNo", new JValue(account.AccountNo)}
            };
            var fields = template.OrderTemplate.EnumerateNotTemplatedFields().ToList();
            foreach (var field in fields)
            {
                form.Add(field.Key, new JValue(field.Value));
            }
            return new PaymentForm(form);
        }
    }
}
