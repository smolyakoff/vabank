using System.Collections.Generic;

namespace VaBank.Services.Contracts.Maintenance.Models
{
    public class AuditLogEntryModel : AuditLogEntryBriefModel
    {
        public AuditLogEntryModel()
        {
            DbActions = new List<DbActionModel>();
        }

        public List<DbActionModel> DbActions { get; set; }
    }
}
