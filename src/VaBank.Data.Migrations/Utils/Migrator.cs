using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SQLite;
using NLog;
namespace VaBank.Data.Migrations.Utils
{
    //TODO: move to database project or migrations project
    public static class Migrator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const string MigrationsAssemblyName = "VaBank.Data.Migrations";

        private const string MaintenanceConnectionStringName = "Vabank.Db.Maintenance";

        private const string MaintenaneTagName = "Maintenance";

        public static void MigrateMaintenanceDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[MaintenanceConnectionStringName].ConnectionString;
            var processorFactory = new SqliteProcessorFactory();
            Migrate(connectionString, processorFactory, new [] {MaintenaneTagName});
        }

        private static void Migrate(string connectionString, 
            MigrationProcessorFactory processorFactory, 
            IEnumerable<string> tags,
            string profile = null, 
            int timeout = 60)
        {
            //TODO: write log somewhere
            var builder = new StringBuilder();
            var sqlLogWriter = new StringWriter(builder);
            var announcer = new TextWriterAnnouncer(sqlLogWriter) {ShowSql = true};
            var assembly = Assembly.GetExecutingAssembly();
            var migrationContext = new RunnerContext(announcer) {Profile = profile, Tags = tags};
            var options = new ProcessorOptions
            {
                PreviewOnly = false, 
                Timeout = timeout
            };
            var factory = processorFactory;
            using (var processor = factory.Create(connectionString, announcer, options))
            {
                var runner = new MigrationRunner(assembly, migrationContext, processor);
                runner.MigrateUp(true);
            }
        }         
    }
}