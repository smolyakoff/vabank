using System;
using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Processing.Models
{
    public class TransactionModel
    {
        public Guid Id { get; set; }

        public string AccountNo { get; set; }

        public string Description { get; set; }

        public ProcessStatusModel Status { get; set; }
    }
}
