using System;

namespace VaBank.Services.Contracts.Maintenance.Commands
{
    public class LogAppActionCommand
    {
        public Guid OperationId { get; set; }

        public DateTime TimestampUtc { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string Data { get; set; }
    }
}
