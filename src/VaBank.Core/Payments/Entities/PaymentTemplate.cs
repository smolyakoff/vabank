using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Payments.Entities
{
    public class PaymentTemplate : OperationCategory
    {
        internal PaymentTemplate()
        {
        }

        public string FormTemplate { get; protected set; }
    }
}
