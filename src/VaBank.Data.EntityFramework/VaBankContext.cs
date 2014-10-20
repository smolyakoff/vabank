using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using VaBank.Common.Data.Database;
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
        private readonly ITransactionProvider _transactionProvider;

        public VaBankContext(IConnectionProvider connectionProvider, ITransactionProvider transactionProvider)
            :base(connectionProvider.Connection, true)
        {
            if (transactionProvider == null)
            {
                throw new ArgumentNullException("transactionProvider", "Transaction provider is null");
            }
            if (transactionProvider.HasCurrentTransaction)
            {
                Database.UseTransaction(transactionProvider.CurrentTransaction);
            }
            _transactionProvider = transactionProvider;
            transactionProvider.TransactionStarted += OnTransactionStarted;
            Database.SetInitializer(new NullDatabaseInitializer<VaBankContext>());
        }


        //Question: do we really need this properties?
        public DbSet<SystemLogEntry> Logs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<ApplicationClient> Clients { get; set; }
        public DbSet<ApplicationToken> Tokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //App mappings registration
            modelBuilder.Configurations.Add(new OperationMap());
            modelBuilder.Configurations.Add(new ResourceMap());
            modelBuilder.Configurations.Add(new FileLinkMap());

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

        private void OnTransactionStarted(object sender, EventArgs eventArgs)
        {
            Database.UseTransaction(_transactionProvider.CurrentTransaction);
        }
    }
}