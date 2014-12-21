namespace VaBank.Services.Contracts.Statistics.Models
{
    public class OverallSystemInfoModel
    {
        public long UsersCount { get; set; }

        public long DbVersion { get; set; }

        public long ProcessedTransactionsCount { get; set; }
    }
}
