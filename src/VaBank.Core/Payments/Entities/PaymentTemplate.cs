using System.Collections.Generic;
using System.Linq;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.Processing.Entities;

namespace VaBank.Core.Payments.Entities
{
    public class PaymentTemplate : OperationCategory
    {
        internal PaymentTemplate()
        {
        }

        public string HierarchicalName
        {
            get { return GetHierarchicalName(); }
        }

        public string FormTemplate { get; protected set; }

        private string GetHierarchicalName()
        {
            OperationCategory category = this;
            var categories = new List<OperationCategory>();
            while (category != null)
            {
                categories.Add(category);
                category = category.Parent;
            }
            categories.Reverse();
            return string.Join(" / ", categories.Select(x => x.Name));
        }
    }
}
