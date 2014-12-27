using System.Collections.Generic;

namespace VaBank.Services.Contracts.Payments.Models
{
    public class PaymentsLookupModel
    {
        public PaymentsLookupModel()
        {
            Categories = new List<PaymentCategoryModel>();
        }

        public List<PaymentCategoryModel> Categories { get; set; } 
    }
}
