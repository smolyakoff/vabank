using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Payments.Entities
{
    public class Payment : Transfer
    {
        internal Payment(
            PaymentOrder order,
            PaymentTemplate template,
            string name,
            string form,
            OperationCategory category, 
            Account from, 
            Account to, 
            Currency currency, 
            decimal amount) : base(category, from, to, currency, amount)
        {
            Argument.NotNull(order, "order");
            Argument.NotEmpty(form, "form");
            Argument.NotNull(template, "template");
            Argument.NotEmpty(name, "name");
            
            Order = order;
            Form = form;
            Template = template;
            Name = name;
        }

        protected Payment()
        {
        }

        public string Name { get; protected set; }

        public virtual PaymentOrder Order { get; protected set; }

        public string Form { get; protected set; }

        public virtual PaymentTemplate Template { get; protected set; }
    }
}
