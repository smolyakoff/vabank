using Autofac;
using Autofac.Integration.WebApi;


namespace VaBank.UI.Web.Modules
{
    public class WebApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(ThisAssembly);
        }
    }
}