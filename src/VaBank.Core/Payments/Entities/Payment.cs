using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Payments.Entities
{
    public class Payment : Transfer
    {
        internal Payment(
            PaymentOrder order,
            string form,
            OperationCategory category, 
            Account from, 
            Account to, 
            Currency currency, 
            decimal amount) : base(category, from, to, currency, amount)
        {
            Argument.NotNull(order, "order");
            Argument.NotEmpty(form, "form");
            
            PaymentOrder = order;
            Form = form;
        }

        protected Payment()
        {
        }

        public virtual PaymentOrder PaymentOrder { get; protected set; }

        public string Form { get; protected set; }
    }
}
