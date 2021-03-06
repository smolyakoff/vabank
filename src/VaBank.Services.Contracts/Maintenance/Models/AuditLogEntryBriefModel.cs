﻿using System;
using System.Collections.Generic;

namespace VaBank.Services.Contracts.Maintenance.Models
{
    public class AuditLogEntryBriefModel
    {
        public AuditLogEntryBriefModel()
        {
            AppActions = new List<AppActionModel>();
        }

        public Guid OperationId { get; set; }

        public Guid? UserId { get; set; }

        public string ApplicationId { get; set; }

        public string UserName { get; set; }

        public DateTime StartedUtc { get; set; }

        public List<AppActionModel> AppActions { get; set; } 
    }
}
