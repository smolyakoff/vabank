using System.Collections.Generic;

namespace VaBank.Services.Contracts.Maintenance.Models
{
    public abstract class SystemLogLookupModel
    {
        protected SystemLogLookupModel()
        {
            Types = new List<string>();
            Levels = new List<string>();
        }

        public List<string> Levels { get; set; }

        public List<string> Types { get; set; }
    }
}
