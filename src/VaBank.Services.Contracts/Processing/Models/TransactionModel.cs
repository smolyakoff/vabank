using System;

namespace VaBank.Services.Contracts.Processing.Models
{
    public class TransactionModel
    {
        public Guid Id { get; set; }

        public string AccountNo { get; set; }
    }
}
