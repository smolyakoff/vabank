using System.Data.Entity.ModelConfiguration;
using VaBank.Core.Membership;
using VaBank.Core.Membership.Entities;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Membership.Mappings
{
    internal class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMap()
        {
            ToTable("UserProfile", "Membership").HasKey(x => x.UserId);
            Property(x => x.FirstName).HasMaxLength(Restrict.Length.Name).IsRequired();
            Property(x => x.LastName).HasMaxLength(Restrict.Length.Name).IsRequired();
            Property(x => x.Email).HasMaxLength(Restrict.Length.Email).IsRequired();
            Property(x => x.PhoneNumber).HasMaxLength(Restrict.Length.PhoneNumber).IsOptional();
            Property(x => x.PhoneNumberConfirmed).IsRequired();
            Property(x => x.SmsConfirmationEnabled).IsRequired();
            Property(x => x.SmsNotificationEnabled).IsRequired();
            Property(x => x.SecretPhrase).HasMaxLength(Restrict.Length.BigString).IsRequired();
            Property(x => x.RowVersion).IsRowVersion();
        }
    }
}