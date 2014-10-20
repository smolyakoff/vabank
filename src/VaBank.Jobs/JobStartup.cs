namespace VaBank.Jobs
{
    public class JobStartup
    {
        public void Start()
        {
            RegisterRecurring();
        }

        private static void RegisterRecurring()
        {
            #if !DEBUG
                VabankJob.AddOrUpdateRecurring<KeepAliveJob>("KeepAlive", "*/10 * * * *");
            #endif
        }
    }
}
