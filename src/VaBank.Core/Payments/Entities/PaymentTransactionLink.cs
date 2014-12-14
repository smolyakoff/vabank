using System;
using VaBank.Common.Validation;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Payments.Entities
{
    public class PaymentTransactionLink
    {
        public PaymentTransactionLink(Transaction transaction, PaymentOrder order)
        {
            Argument.NotNull(transaction, "transaction");
            Argument.NotNull(order, "order");

            Order = order;
            Transaction = transaction;
            TransactionId = Transaction.Id;
        }

        public virtual Guid TransactionId { get; protected set; }

        public virtual PaymentOrder Order { get; protected set; }

        public virtual Transaction Transaction { get; protected set; }
    }
}
