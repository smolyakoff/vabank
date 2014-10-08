using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Membership;

namespace VaBank.Data.EntityFramework.Membership.Mappings
{
    internal class ApplicationTokenMap : EntityTypeConfiguration<ApplicationToken>
    {
        public ApplicationTokenMap()
        {
            ToTable("ApplicationToken", "Membership").HasKey(x => x.Id);
            HasRequired(x => x.Client).WithRequiredDependent();
            Property(x => x.ClientId).HasColumnName("ClientID");
            Property(x => x.IssuedUtc).IsRequired();
            Property(x => x.ExpiresUtc).IsRequired();
            Property(x => x.ProtectedTicket).HasMaxLength(RestrictionConstants.SecurityStringLength)
                .IsRequired();
            Property(x => x.Subject).IsRequired().HasMaxLength(50);
        }
    }
}