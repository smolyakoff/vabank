namespace VaBank.Services.Contracts.Payments.Models
{
    public class PaymentCategoryUsagesModel
    {
        public PaymentCategoryModel Category { get; set; }

        public int Usages { get; set; }
    }
}
