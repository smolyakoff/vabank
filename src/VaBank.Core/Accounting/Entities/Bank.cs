using System.Collections.Generic;
using VaBank.Common.Validation;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting.Entities
{
    public class Bank : Entity
    {
        internal Bank(string code, string name)
        {
            Argument.NotEmpty(code, "code");
            Argument.NotEmpty(name, "name");
            Argument.Satisfies(parent.Code, x => x != code, "parent", "Parent bank code can't be the same as creating bank code.");

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
