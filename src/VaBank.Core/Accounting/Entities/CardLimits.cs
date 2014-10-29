namespace VaBank.Core.Accounting.Entities
{
    public class CardLimits
    {
        internal CardLimits()
        {
        }

        public int OperationsPerDayLocal { get; set; }

        public int OperationsPerDayAbroad { get; set; }

        public decimal AmountPerDayLocal { get; set; }

        public decimal AmountPerDayAbroad { get; set; }
    }
}
