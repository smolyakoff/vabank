namespace VaBank.Jobs.Configuration
{
    public class ReccuringJobConfig : IJobConfig
    {
        public string CronExpression { get; protected set; }
    }
}
