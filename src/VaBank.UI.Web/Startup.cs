using System.Configuration;
using System.Net;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.StaticFiles.Infrastructure;
using NLog;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using SquishIt.Framework;
using SquishIt.Sass;
using VaBank.UI.Web.Views;

namespace VaBank.UI.Web
{    
    public class Startup
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private const string EveryTenMinutes = "*/10 * * * *";

        public void Configuration(IAppBuilder config)
        {
            Bundle.RegisterStylePreprocessor(new SassPreprocessor());

            config.UseStaticFiles("/Client");
            config.UseHangfire(ConfigureHangfire);
            config.Use(Handler);

            _logger.Info("Application is started!");
            #if !DEBUG
                RecurringJob.AddOrUpdate("KeepAlive", () => KeepAlive(), EveryTenMinutes);
            #endif
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

        public static void KeepAlive()
        {
            var client = new WebClient();
            client.DownloadData("https://vabank.azurewebsites.net");
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Keep alive was triggered.");
        }
    }
}