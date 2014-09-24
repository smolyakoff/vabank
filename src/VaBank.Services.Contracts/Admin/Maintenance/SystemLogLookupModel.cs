using System.Collections.Generic;

namespace VaBank.Services.Contracts.Admin.Maintenance
{
    public abstract class SystemLogLookupModel
    {
        protected SystemLogLookupModel()
        {
            Levels = new List<SystemLogLevelModel>();
            Types = new List<string>();
        }

        public List<SystemLogLevelModel> Levels { get; protected set; }

        public List<string> Types { get; set; }
    }
}
