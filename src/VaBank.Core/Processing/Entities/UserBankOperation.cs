using VaBank.Common.Validation;
using VaBank.Core.Common;
using VaBank.Core.Membership.Entities;

namespace VaBank.Core.Processing.Entities
{
    public class UserBankOperation : Entity
    {
        public UserBankOperation(BankOperation operation, User user)
        {
            Argument.NotNull(operation, "operation");
            Argument.NotNull(user, "user");

            Operation = operation;
            User = user;
            OperationId = operation.Id;
        }

        public long OperationId { get; set; }

        public virtual BankOperation Operation { get; private set; }

        public virtual User User { get; private set; }
    }
}
