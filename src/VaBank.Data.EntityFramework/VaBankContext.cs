using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using VaBank.Core.Entities;
using VaBank.Data.EntityFramework.Mappings;
using VaBank.Core.Entities.Membership;

namespace VaBank.Data.EntityFramework
{
    public class VaBankContext: DbContext
    {
        public VaBankContext(): base("Name=VaBank.Db")
        {
        }

        public VaBankContext(string connectionStringName): base("Name=" + connectionStringName)
        {
        }

        public DbSet<Log> Logs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<ApplicationClient> Clients { get; set; }
        public DbSet<ApplicationToken> Tokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new LogMap());
            
            //Membership mappings registration
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserProfileMap());
            modelBuilder.Configurations.Add(new UserClaimMap());
            modelBuilder.Configurations.Add(new ApplicationClientMap());
            modelBuilder.Configurations.Add(new ApplicationTokenMap());
        }
    }
}
