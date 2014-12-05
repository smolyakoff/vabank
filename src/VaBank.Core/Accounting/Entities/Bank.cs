using VaBank.Common.Validation;
using VaBank.Core.Common;

namespace VaBank.Core.Accounting.Entities
{
    public class Bank : Entity
    {
        internal Bank(string code, string name, Bank parent)
        {
            Argument.NotEmpty(code, "code");
            Argument.NotEmpty(name, "name");
            Argument.NotNull(parent, "parent");
            Argument.Satisfies(parent.Code, x => x != code, "parent", "Parent bank code can't be the same as creating bank code.");

            Code = code;
            Name = name;
            Parent = parent;
        }

        protected Bank()
        {
        }

        public string Code { get; protected set; }

        public Bank Parent { get; protected set; }

        public string Name { get; set; }
    }
}
