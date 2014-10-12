namespace VaBank.Services.Contracts.Maintenance.Models
{
    public class SystemLogExceptionModel
    {
        public long EventId { get; set; }

        public string Source { get; set; }

        public string Exception { get; set; }
    }
}
