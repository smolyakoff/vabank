namespace VaBank.Jobs.Configuration
{
    public interface IJobConfigProvider
    {
        TJobConfig Get<TJobConfig>(string jobName) where TJobConfig : class, IJobConfig;
    }
}
