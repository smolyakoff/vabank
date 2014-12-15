using Newtonsoft.Json.Linq;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Payments.Entities
{
    public class Payment : Transfer
    {
        internal Payment(
            PaymentTemplate template,
            JObject submittedForm,
            PaymentOrder order,
            Currency currency,
            Account from, 
            Account to) : base(template, from, to, currency, order.Amount)
        {
            Argument.NotNull(submittedForm, "submittedForm");
            Argument.NotNull(template, "template");
            Argument.NotNull(order, "order");

            Order = order;
            Form = submittedForm.ToString();
        }

        protected Payment()
        {
        }

        public string TemplateCode
        {
            get { return Category.Code; }
        }

        public virtual PaymentOrder Order { get; protected set; }

        public string Form { get; protected set; }

        public string Info { get; internal set; }
    }
}
