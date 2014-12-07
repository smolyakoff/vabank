using System;

using VaBank.Services.Contracts.Accounting.Models;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Processing.Models
{
    public class TransactionModel
    {
        public Guid Id { get; set; }

        public string AccountNo { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public decimal TransactionAmount { get; set; }

        public CurrencyModel Currency { get; set; }

        public ProcessStatusModel Status { get; set; }
    }
}
