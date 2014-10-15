using System;
using HangFire;
using NLog;

namespace VaBank.Jobs
{
    public abstract class Job<T>
    {
        protected readonly Logger Logger;

        protected Job()
        {
            Logger = LogManager.GetCurrentClassLogger();
        } 

        public void Execute(T arguments, IJobCancellationToken token)
        {
            try
            {
                Do(arguments, token);
            }
            catch (OperationCanceledException)
            {
                //TODO: log cancellation here
            }
            catch (Exception ex)
            {
                //TODO: log exception here
            }
        }

        protected string JobName
        {
            get { return "SomeJobName"; }
        }

        protected abstract void Do(T argument, IJobCancellationToken cancellationToken);
    }
}
