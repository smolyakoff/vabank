using System.Collections.Generic;

namespace VaBank.Services.Contracts.Maintenance.Models
{
    public class AuditLogLookupModel
    {
        public AuditLogLookupModel()
        {
            Codes = new List<string>();
        }

        public List<string> Codes { get; set; }
    }
}
