using VaBank.Core.Accounting.Entities;
using VaBank.Core.Common;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Payments.Entities
{
    public class PaymentTemplate : Entity
    {
        internal PaymentTemplate()
        {
        }

        public string Code { get; private set; }

        public virtual OperationCategory Category { get; protected set; }

        public virtual Account Account { get; protected set; }

        public virtual Currency Currency { get; protected set; }

        public string Name { get; protected set; }

        public string Form { get; protected set; }
    }
}
