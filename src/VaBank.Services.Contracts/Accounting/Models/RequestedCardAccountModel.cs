namespace VaBank.Services.Contracts.Accounting.Models
{
    public class RequestedCardAccountModel
    {
        /// <summary>
        /// Card number for new card
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// Account number for new card
        /// </summary>
        public string AccountNo { get; set; }

        /// <summary>
        /// Default card limits
        /// </summary>
        public CardLimitsModel Limits { get; set; }
    }
}
