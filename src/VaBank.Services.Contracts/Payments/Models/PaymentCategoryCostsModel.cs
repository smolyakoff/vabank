using System.Collections.Generic;
using VaBank.Services.Contracts.Accounting.Models;

namespace VaBank.Services.Contracts.Payments.Models
{
    public class PaymentCategoryCostsModel
    {
        public PaymentCategoryCostsModel()
        {
            Data = new List<PaymentCategoryCostsItemModel>();
        }

        public CardNameModel Card { get; set; }

        public CurrencyModel Currency { get; set; }

        public List<PaymentCategoryCostsItemModel> Data { get; set; }

        public decimal Total { get; set; }
    }
}
