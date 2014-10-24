using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac;
using Autofac.Integration.WebApi;


namespace VaBank.UI.Web.Modules
{
    public class WebApiModule : Module
    {
        private readonly HttpConfiguration _httpConfiguration;

        public WebApiModule(HttpConfiguration httpConfiguration)
        {
            if (httpConfiguration == null)
            {
                throw new ArgumentNullException("httpConfiguration");
            }
            _httpConfiguration = httpConfiguration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<DataAccessModule>();
            builder.RegisterModule<ServicesModule>();
            builder.RegisterApiControllers(ThisAssembly);
        }
    }
}