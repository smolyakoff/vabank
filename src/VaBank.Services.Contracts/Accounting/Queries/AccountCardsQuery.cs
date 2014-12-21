namespace VaBank.Services.Contracts.Accounting.Queries
{
    public class AccountCardsQuery
    {
        public string AccountNo { get; set; }

        public bool? IsActive { get; set; }
    }
}
