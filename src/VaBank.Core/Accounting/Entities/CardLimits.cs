namespace VaBank.Core.Accounting.Entities
{
    public class CardLimits
    {
        internal CardLimits()
        {
        }

        public int LimitOperationsPerDayLocal { get; set; }

        public int LimitOperationsPerDayAbroad { get; set; }

        public decimal LimitAmountPerDayLocal { get; set; }

        public decimal LimitAmountPerDayAbroad { get; set; }
    }
}
