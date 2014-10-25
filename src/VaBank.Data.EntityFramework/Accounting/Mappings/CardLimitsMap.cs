using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Accounting.Entities;

namespace VaBank.Data.EntityFramework.Accounting.Mappings
{
    public class CardLimitsMap : ComplexTypeConfiguration<CardLimits>
    {
        public CardLimitsMap()
        {
            Property(x => x.LimitAmountPerDayAbroad).HasColumnName("LimitAmountPerDayAbroad").IsRequired();
            Property(x => x.LimitAmountPerDayLocal).HasColumnName("LimitAmountPerDayLocal").IsRequired();
            Property(x => x.LimitOperationsPerDayAbroad).HasColumnName("LimitOperationsPerDayAbroad").IsRequired();
            Property(x => x.LimitOperationsPerDayLocal).HasColumnName("LimitOperationsPerDayLocal").IsRequired();
        }
    }
}
