using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Payments.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Processing.Resources
{
    [Injectable(Lifetime = Lifetime.Singleton)]
    public class TransactionReferenceBook
    {
        public TransactionName CompensationFor(Transaction transaction)
        {
            Argument.NotNull(transaction, "transaction");
            return new TransactionName("COMPENSATION", Descriptions.Compensation, transaction.Id);
        }

        public TransactionName ForOperation(BankOperation operation)
        {
            Argument.NotNull(operation, "operation");
            return new TransactionName(operation.Category.Code, operation.Category.Name);
        }

        public TransactionName ForPayment(PaymentTemplate template)
        {
            Argument.NotNull(template, "template");
            return new TransactionName(template.Code, template.DisplayName);
        }
    }
}
