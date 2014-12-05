namespace VaBank.Services.Contracts.Payments.Models
{
    public class PaymentArchiveDetailsModel : PaymentArchiveItemModel
    {
        public PaymentOrderModel Order { get; set; }
    }
}
