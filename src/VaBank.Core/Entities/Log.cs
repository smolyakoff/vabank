using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Core.Entities
{
    public class Log: Entity<long>
    {
        public string Application { get; set; }
        public DateTime TimeStampUtc { get; set; }
        public string Level { get; set; }
        public string Type { get;set; }
        public string User { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Source { get; set; }
    }
}
