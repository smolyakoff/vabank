using System;

namespace VaBank.Core.App
{
    public class ApplicationAction
    {
        public Guid EventId { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string Data { get; set; }
    }
}
