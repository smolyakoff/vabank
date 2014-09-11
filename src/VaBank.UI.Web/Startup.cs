using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;

namespace VaBank.UI.Web
{    
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            builder.Use(new Func<AppFunc, AppFunc>(next => (AppFunc)Invoke));            
        }

        public Task Invoke(IDictionary<string, object> env)
        {
            var response = new OwinResponse(env);
            return response.WriteAsync("Hello, World!");
        }
    }
}