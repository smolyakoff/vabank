using System;
using System.Data.Entity;
using System.Reflection;
using VaBank.Common.Data.Database;
using VaBank.Core.Common;

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
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