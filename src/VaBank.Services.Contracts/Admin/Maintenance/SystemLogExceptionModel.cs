namespace VaBank.Services.Contracts.Admin.Maintenance
{
    public class SystemLogExceptionModel
    {
        public long EventId { get; set; }

        public string Source { get; set; }

        public string Exception { get; set; }
    }
}
