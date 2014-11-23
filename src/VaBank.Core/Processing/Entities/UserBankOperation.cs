using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Processing.Entities
{
    public class UserBankOperation : Entity
    {
        private BankOperation _operation;

        public UserBankOperation(BankOperation operation, User user)
        {
            Argument.NotNull(operation, "operation");
            Argument.NotNull(user, "user");

            Operation = operation;
            User = user;
            OperationId = operation.Id;
        }

        protected UserBankOperation()
        {
        }

        public long OperationId { get; set; }

        public virtual BankOperation Operation
        {
            get { return _operation; }
            set
            {
                if (value == null)
                {
                    _operation = null;
                }
                else
                {
                    _operation = value;
                    OperationId = _operation.Id;
                }
            }
        }

        public virtual User User { get; private set; }
    }
}
