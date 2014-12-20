namespace VaBank.Services.Contracts.Statistics.Models
{
    public class OverallSystemInfoModel
    {
        public long UsersCount { get; set; }

        public string ServerVersion { get; set; }

        public long ProcessedTransactionsCount { get; set; }
    }
}
