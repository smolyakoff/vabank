using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Autofac;
using Hangfire;
using Hangfire.SqlServer;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NLog;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using SquishIt.Framework;
using SquishIt.Sass;
using VaBank.Common.Data;
using VaBank.UI.Web.Api.Infrastructure.ModelBinding;
using VaBank.UI.Web.Middleware;
using VaBank.UI.Web.Modules;
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
            config.UseAutofacMiddleware(ConfigureAutofac());

            config.UseStaticFiles("/Client");
            config.UseHangfire(ConfigureHangfire);
            config.Use<CultureMiddleware>();

            var httpConfig = ConfigureWebApi();
            config.UseWebApi(httpConfig);
            config.UseAutofacWebApi(httpConfig);
            
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

        private static HttpConfiguration ConfigureWebApi()
        {
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();

            //Formatters
            configuration.Formatters.Clear();
            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
            var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
            serializerSettings.Converters.Add(new StringEnumConverter());
            jsonFormatter.SerializerSettings = serializerSettings;
            configuration.Formatters.Add(jsonFormatter);

            //Model Binders
            configuration.Services.Insert(
                typeof(ModelBinderProvider), 0, new InheritanceAwareModelBinderProvider(typeof(IQuery), new QueryModelBinder()));

            configuration.EnsureInitialized();
            return configuration;
        }

        private static ILifetimeScope ConfigureAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<DataAccessModule>();
            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule<WebApiModule>();
            var container = builder.Build();
            return container;
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