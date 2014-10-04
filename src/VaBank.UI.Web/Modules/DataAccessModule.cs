using System.Data.Entity;
using Autofac;
using VaBank.Common.Data.Repositories;
using VaBank.Core.Common;
using VaBank.Data.EntityFramework;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.UI.Web.Modules
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Entity framework data access module registration
            builder.RegisterType<VaBankContext>().As<DbContext>().As<IUnitOfWork>().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof (Repository<>).Assembly)
                .Where(t => typeof (IRepository).IsAssignableFrom(t) && !t.IsGenericType)
                .AsImplementedInterfaces()
                .InstancePerRequest();
            builder.RegisterGeneric(typeof (Repository<>))
                .As(typeof (IRepository<>))
                .As(typeof (IQueryRepository<>))
                .InstancePerRequest();
        }
    }
}