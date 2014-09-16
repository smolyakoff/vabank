using System.Configuration;
using Hangfire;
using Hangfire.SqlServer;
using NLog;
using NLog.Common;
using NLog.Targets;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using VaBank.Data.Migrations.Utils;
using VaBank.UI.Web.Views;

namespace VaBank.UI.Web
{    
    public class Startup
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Configuration(IAppBuilder config)
        {
            Migrator.MigrateMaintenanceDatabase();

            config.UseHangfire(ConfigureHangfire);
            config.Use(Handler);

            _logger.Info("Application is started!");
            RecurringJob.AddOrUpdate("Dummy", () => DummyJob(), Cron.Hourly);
        }

        public Task Handler(IOwinContext context, Func<Task> next)
        {
            var response = context.Response;
            var template = new Index();
            return response.WriteAsync(template.TransformText());
        }

        private static void ConfigureHangfire(IBootstrapperConfiguration config)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Vabank.Db"].ConnectionString;
            config.UseDashboardPath("/admin/hangfire");
            var storageOptions = new SqlServerStorageOptions {QueuePollInterval = TimeSpan.FromMinutes(1)};
            config.UseStorage(new SqlServerStorage(connectionString, storageOptions));
            config.UseServer();
        }

        public static void DummyJob()
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Hangfire job is triggered");
        }
    }
}