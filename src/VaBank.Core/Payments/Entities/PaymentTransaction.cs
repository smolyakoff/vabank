using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Payments.Entities
{
    public class PaymentTransaction : Transaction
    {
        internal PaymentTransaction(
            PaymentOrder order,
            Account account,
            Currency transactionCurrency,
            decimal transactionAmount,
            decimal accountAmount,
            string code,
            string description,
            string location
            )
            : base(account, transactionCurrency, transactionAmount,
                accountAmount, code, description, location)
        {
            Argument.NotNull(order, "order");
            Order = order;
        }

        protected PaymentTransaction()
        {
        }

        public PaymentOrder Order { get; protected set; }
    }
}
