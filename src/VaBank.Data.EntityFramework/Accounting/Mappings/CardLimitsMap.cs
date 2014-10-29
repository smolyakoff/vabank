using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Data.EntityFramework.Accounting.Mappings
{
    public class CardLimitsMap : ComplexTypeConfiguration<CardLimits>
    {
        public CardLimitsMap()
        {
            Property(x => x.AmountPerDayAbroad).HasColumnName("LimitAmountPerDayAbroad").IsRequired();
            Property(x => x.AmountPerDayLocal).HasColumnName("LimitAmountPerDayLocal").IsRequired();
            Property(x => x.OperationsPerDayAbroad).HasColumnName("LimitOperationsPerDayAbroad").IsRequired();
            Property(x => x.OperationsPerDayLocal).HasColumnName("LimitOperationsPerDayLocal").IsRequired();
        }
    }
}
