using System.Data.Entity;
using VaBank.Core.Common;
using VaBank.Core.Maintenance;
using VaBank.Core.Membership;
using VaBank.Data.EntityFramework.App.Mappings;
using VaBank.Data.EntityFramework.Maintenance.Mappings;
using VaBank.Data.EntityFramework.Membership.Mappings;

namespace VaBank.Data.EntityFramework
{
    public class VaBankContext : DbContext, IUnitOfWork
    {
        public VaBankContext() : base("Name=Vabank.Db")
        {
            Database.SetInitializer(new NullDatabaseInitializer<VaBankContext>());
        }

        public DbSet<SystemLogEntry> Logs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<ApplicationClient> Clients { get; set; }
        public DbSet<ApplicationToken> Tokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //App mappings registration
            modelBuilder.Configurations.Add(new OperationMarkerMap());

            //Maintenance mappings registration
            modelBuilder.Configurations.Add(new SystemLogEntryMap());

            //Membership mappings registration
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserProfileMap());
            modelBuilder.Configurations.Add(new UserClaimMap());
            modelBuilder.Configurations.Add(new ApplicationClientMap());
            modelBuilder.Configurations.Add(new ApplicationTokenMap());
        }

        public void Commit()
        {
            SaveChanges();
        }
    }
}