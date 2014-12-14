using Newtonsoft.Json.Linq;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Core.Payments.Entities
{
    public class CardPayment : Payment
    {
        internal CardPayment(
            Card card, 
            PaymentTemplate template,
            PaymentOrder order,
            JObject submittedForm,
            Account from,
            Account to,
            Currency currency) 
            : base(template, submittedForm, order, currency, from, to)
        {
            Argument.NotNull(card, "card");
            Card = card;
        }

        protected CardPayment()
        {
        }
        
        public virtual Card Card { get; protected set; } 
    }
}
