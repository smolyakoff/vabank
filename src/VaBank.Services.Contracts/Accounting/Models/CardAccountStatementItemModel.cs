using System;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CardAccountStatementItemModel
    {
        public Guid TransactionId { get; set; }

        public string AccountNo { get; set; }

        public CustomerCardBriefModel Card { get; set; }

        public ProcessStatusModel Status { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public DateTime? PostDateUtc { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public CurrencyModel Currency { get; set; }

        public decimal TransactionAmount { get; set; }

        public decimal AccountAmount { get; set; }

        public decimal RemainingBalance { get; set; }

        public string ErrorMessage { get; set; }
    }
}
