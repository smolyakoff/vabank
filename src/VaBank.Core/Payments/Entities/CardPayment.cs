using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Payments.Entities
{
    public class CardPayment : Payment
    {
        internal CardPayment(
            Card card, 
            PaymentOrder order,
            PaymentTemplate template,
            string name,
            string form,
            OperationCategory category,
            Account from,
            Account to,
            Currency currency,
            decimal amount
            ) : base(order, template, name, form, category, from, to, currency, amount)
        {
            Argument.NotNull(card, "card");
            Card = card;
        }

        protected CardPayment()
        {
        }
        
        public Card Card { get; protected set; } 
    }
}
