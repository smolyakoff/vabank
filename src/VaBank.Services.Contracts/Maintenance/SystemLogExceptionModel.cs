namespace VaBank.Services.Contracts.Maintenance
{
    public class SystemLogExceptionModel
    {
        public long EventId { get; set; }

        public string Source { get; set; }

        public string Exception { get; set; }
    }
}
