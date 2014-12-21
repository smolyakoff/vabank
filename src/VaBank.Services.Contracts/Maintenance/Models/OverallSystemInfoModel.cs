namespace VaBank.Services.Contracts.Maintenance.Models
{
    public class OverallSystemInfoModel
    {
        public long UsersCount { get; set; }

        public long DbVersion { get; set; }

        public long CompletedBankOperationsCount { get; set; }
    }
}
