using System.Collections.Generic;
using VaBank.Common.Validation;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting.Entities
{
    public class Bank : Entity
    {
        private Bank _parent;
        
        internal Bank(string code, string name)
        {
            Argument.NotEmpty(code, "code");
            Argument.NotEmpty(name, "name");

            Code = code;
            Name = name;
            ChildrenBanks = new List<Bank>();
        }

        protected Bank()
        {
        }

        public string Code { get; protected set; }

        public virtual Bank Parent { get; set; }

        public virtual ICollection<Bank> ChildrenBanks { get; set; }

        public string Name { get; set; }
    }
}
