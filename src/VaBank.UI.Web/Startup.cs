using System.Configuration;
using System.IO;
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
            EnsureApplicationDataExists();

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
            //TODO: do auth as it's generally dangerous to keep no auth with hangfire
            config.UseAuthorizationFilters();
            config.UseServer();
        }

        private static void EnsureApplicationDataExists()
        {
            var applicationData = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
            if (string.IsNullOrEmpty(applicationData))
            {
                return;
            }
            if (!Directory.Exists(applicationData))
            {
                Directory.CreateDirectory(applicationData);
            }
        }

        public static void DummyJob()
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Hangfire job is triggered");
        }
    }
}