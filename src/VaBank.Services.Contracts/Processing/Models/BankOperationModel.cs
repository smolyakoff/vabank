using VaBank.Services.Contracts.Common.Models;

namespace VaBank.Services.Contracts.Processing.Models
{
    public class BankOperationModel
    {
        public long Id { get; set; }

        public string CategoryCode { get; set; }

        public ProcessStatusModel Status { get; set; }
    }
}
