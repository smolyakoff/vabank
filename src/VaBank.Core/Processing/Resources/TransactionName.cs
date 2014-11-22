using VaBank.Common.Validation;

namespace VaBank.Core.Processing.Resources
{
    public class TransactionName
    {
        internal TransactionName(string code, string description, params object[] args)
        {
            Argument.NotEmpty(code, "code");
            Argument.NotEmpty(description, "description");

            Code = code;
            Description = string.Format(description, args);
        }

        public string Code { get; private set; }

        public string Description { get; private set; }
    }
}
