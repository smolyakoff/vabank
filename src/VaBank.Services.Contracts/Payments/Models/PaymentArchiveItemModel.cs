using System;
using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Payments.Models
{
    public class PaymentArchiveItemModel
    {
        public long OperationId { get; set; }

        public CardNameModel Card { get; set; }

        public string PaymentCode { get; set; }

        public string PaymentName { get; set; }

        public string ParentCategoryName { get; set; }

        public DateTime DateUtc { get; set; }

        public CurrencyModel Currency { get; set; }

        public decimal Amount { get; set; }

        public ProcessStatusModel Status { get; set; }

        public string Info { get; set; }
    }
}
