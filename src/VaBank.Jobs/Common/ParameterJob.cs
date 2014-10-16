using Autofac;

namespace VaBank.Jobs.Common
{
    public abstract class ParameterJob<TContext, T> : BaseJob<TContext>
        where TContext : class, IJobContext<T>
    {
        protected ParameterJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override TContext CreateContext(ILifetimeScope scope, object data)
        {
            var context = base.CreateContext(scope, data);
            context.Data = (T) data;
            return context;
        }
    }
}
