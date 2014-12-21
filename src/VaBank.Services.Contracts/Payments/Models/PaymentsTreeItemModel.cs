using System.Collections.Generic;

namespace VaBank.Services.Contracts.Payments.Models
{
    public class PaymentsTreeItemModel
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public List<PaymentsTreeItemModel> Children { get; set; }
    }
}
