using System.Collections.Generic;
using VaBank.Common.Validation;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting.Entities
{
    public class Bank : Entity
    {
        internal Bank(string code, string name)
            :this()
        {
            Argument.NotEmpty(code, "code");
            Argument.NotEmpty(name, "name");

            Code = code;
            Name = name;
            
        }

        protected Bank()
        {
            Departments = new List<Bank>();
        }

        public string Code { get; protected set; }

        public virtual Bank Parent { get; protected set; }

        public virtual ICollection<Bank> Departments { get; set; }

        public string Name { get; set; }
    }
}
