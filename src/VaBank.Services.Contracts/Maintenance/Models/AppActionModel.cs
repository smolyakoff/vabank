using System;

namespace VaBank.Services.Contracts.Maintenance.Models
{
    public class AppActionModel
    {
        public Guid EventId { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string JsonData { get; set; }
    }
}
