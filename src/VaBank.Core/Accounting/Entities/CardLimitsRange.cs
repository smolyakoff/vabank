using VaBank.Common.Util;

namespace VaBank.Core.Accounting.Entities
{
    public class CardLimitsRange
    {
        internal CardLimitsRange()
        {
        }

        public Range<decimal> AmountPerDayLocal { get; set; }

        public Range<decimal> AmountPerDayAbroad { get; set; }
    }
}
