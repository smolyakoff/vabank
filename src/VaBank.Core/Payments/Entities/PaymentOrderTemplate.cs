using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VaBank.Common.Validation;
using VaBank.Core.Common;

namespace VaBank.Core.Payments.Entities
{
    public class PaymentOrderTemplate : Entity
    {
        private static readonly Regex PlaceholderRegex = new Regex(@"\{\{.+\}\}"); 

        internal PaymentOrderTemplate()
        {
        }

        public string TemplateCode { get; protected set; }

        public string PayerName { get; internal set; }

        public string PayerBankCode { get; internal set; }

        public string PayerAccountNo { get; internal set; }

        public string PayerTIN { get; internal set; }

        public string BeneficiaryName { get; internal set; }

        public string BeneficiaryBankCode { get; internal set; }

        public string BeneficiaryAccountNo { get; internal set; }

        public string BeneficiaryTIN { get; internal set; }

        public string Purpose { get; internal set; }

        public string Amount { get; internal set; }

        public string CurrencyISOName { get; internal set; }

        public string PaymentCode { get; internal set; }

        public IEnumerable<KeyValuePair<string, string>> EnumerateNotTemplatedFields()
        {
            var properties = GetType().GetProperties();
            var values = properties.Select(x =>
            {
                var value = x.GetValue(this);
                var stringValue = value == null ? null : value.ToString();
                return new KeyValuePair<string, string>(x.Name, stringValue);
            });
            return values.Where(x => x.Value != null && !PlaceholderRegex.IsMatch(x.Value));
        }

        public PaymentOrder CreateOrder(PaymentForm form)
        {
            Argument.NotNull(form, "form");

            var order = new PaymentOrder
            {
                Amount = form.RenderValueOrDefault<decimal>(Amount),
                BeneficiaryAccountNo = form.RenderValueOrDefault<string>(BeneficiaryAccountNo),
                BeneficiaryBankCode = form.RenderValueOrDefault<string>(BeneficiaryBankCode),
                BeneficiaryName = form.RenderValueOrDefault<string>(BeneficiaryName),
                BeneficiaryTIN = form.RenderValueOrDefault<string>(BeneficiaryTIN),
                CurrencyISOName = form.RenderValueOrDefault<string>(CurrencyISOName),
                PayerAccountNo = form.RenderValueOrDefault<string>(PayerAccountNo),
                PayerBankCode = form.RenderValueOrDefault<string>(PayerBankCode),
                PayerName = form.RenderValueOrDefault<string>(PayerName),
                PayerTIN = form.RenderValueOrDefault<string>(PayerTIN),
                PaymentCode = form.RenderValueOrDefault<string>(PaymentCode),
                Purpose = form.RenderValueOrDefault<string>(Purpose)
            };
            return order;
        }
    }
}
