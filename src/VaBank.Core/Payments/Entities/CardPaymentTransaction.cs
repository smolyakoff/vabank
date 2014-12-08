using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Payments.Entities
{
    public class CardPaymentTransaction : CardTransaction
    {
        internal CardPaymentTransaction(
            PaymentOrder order,
            Card card,
            Account account,
            Currency transactionCurrency,
            decimal transactionAmount,
            decimal accountAmount,
            string code,
            string description,
            string location
            )
            : base(code, description, location, account, card, transactionCurrency, transactionAmount, accountAmount)
        {
            Argument.NotNull(order, "order");
            Order = order;
        }

        protected CardPaymentTransaction()
        {
        }

        public PaymentOrder Order { get; protected set; }
    }
}
