using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaBank.Core.Entities
{
    public class Entity<TKey>
    {
        public TKey ID { get; set; }
    }
}
