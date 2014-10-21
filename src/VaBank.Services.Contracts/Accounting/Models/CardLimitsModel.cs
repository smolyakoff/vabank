namespace VaBank.Services.Contracts.Accounting.Models
{
    public class CardLimitsModel
    {
        public int OperationsPerDayLocal { get; set; }

        public int OperationsPerDayAbroad { get; set; }

        public decimal AmountPerDayLocal { get; set; }

        public decimal AmountPerDayAbroad { get; set; }
    }
}
