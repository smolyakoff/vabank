using Autofac;
using VaBank.Jobs.Common;

namespace VaBank.Jobs
{
    public class ApplicationStartedJob : EventListenerJob<DefaultJobContext<ApplicationStartedEvent>, ApplicationStartedEvent>
    {
        public ApplicationStartedJob(ILifetimeScope scope) : base(scope)
        {
        }

        protected override void Execute(DefaultJobContext<ApplicationStartedEvent> context)
        {
            Logger.Info("Jobs are started!");   
        }
    }
}
