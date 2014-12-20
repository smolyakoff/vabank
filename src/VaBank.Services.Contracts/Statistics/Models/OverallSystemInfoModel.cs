namespace VaBank.Services.Contracts.Statistics.Models
{
    public class OverallSystemInfoModel
    {
        public int UsersCount { get; set; }

        public DatabaseInfoModel Database { get; set; }

        public long ProcessedTransactionsCount { get; set; }
    }
}
