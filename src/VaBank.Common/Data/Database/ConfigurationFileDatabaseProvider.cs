using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace VaBank.Common.Data.Database
{
    public class ConfigurationFileDatabaseProvider : IDatabaseProvider, ITransactionFactory, IDisposable
    {
        private readonly DbProviderFactory _factory;

        private readonly string _connectionString;

        private readonly Lazy<DbConnection> _connection;

        public ConfigurationFileDatabaseProvider(string connectionStringName)
        {
            CurrentTransaction = null;
            if (string.IsNullOrEmpty(connectionStringName))
            {
                throw new ArgumentNullException("connectionStringName");
            }
            var settings = ConfigurationManager.ConnectionStrings[connectionStringName];
            _factory = DbProviderFactories.GetFactory(settings.ProviderName);
            _connectionString = settings.ConnectionString;
            _connection = new Lazy<DbConnection>(OpenConnection);
        }

        public DbConnection Connection
        {
            get { return _connection.Value; }
        }

        public DbTransaction CurrentTransaction { get; private set; }

        public bool HasCurrentTransaction
        {
            get { return CurrentTransaction != null; }
        }

        public event EventHandler TransactionStarted;

        public DbDataAdapter CreateAdapter()
        {
            var adapter = _factory.CreateDataAdapter();
            return adapter;
        }

        public DbCommand CreateCommand()
        {
            var command = _factory.CreateCommand();
            if (command != null)
            {
                command.Connection = Connection;
                if (HasCurrentTransaction)
                {
                    command.Transaction = CurrentTransaction;
                }
            }
            return command;
        }

        public DbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            if (CurrentTransaction != null)
            {
                throw new InvalidOperationException("Transaction has already begun.");
            }
            CurrentTransaction = Connection.BeginTransaction();
            TransactionStarted(this, new EventArgs());
            return CurrentTransaction;
        }

        public void Dispose()
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Dispose();
            }
            if (_connection.IsValueCreated && _connection.Value.State != ConnectionState.Closed)
            {
                _connection.Value.Close();
            }
        }

        private DbConnection OpenConnection()
        {
            var connection = _factory.CreateConnection();
            if (connection == null)
            {
                throw new InvalidOperationException("Db provider factory returned null for connection!");
            }
            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }
    }
}
