namespace VaBank.Services.Contracts.Processing.Models
{
    public class TransferModel
    {
        public TransferStatus Status { get; set; }

        public string Description { get; set; }
    }
}
