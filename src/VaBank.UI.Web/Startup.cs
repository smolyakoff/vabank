using System.IO;
using Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin;
using VaBank.UI.Web.Utils;
using VaBank.UI.Web.Views;

namespace VaBank.UI.Web
{    
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            builder.Use(new Func<AppFunc, AppFunc>(next => (AppFunc)Invoke)); 
           
            Migrator.MigrateMaintenanceDatabase();
        }

        public Task Invoke(IDictionary<string, object> env)
        {
            var response = new OwinResponse(env);
            var template = new Index();
            return response.WriteAsync(template.TransformText());
        }
    }
}