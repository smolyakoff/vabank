using System;
using VaBank.Common.IoC;
using VaBank.Common.Validation;
using VaBank.Core.Processing.Entities;

namespace VaBank.Services.Processing.Transactions.Policies
{
    [Injectable(Lifetime = Lifetime.Singleton)]
    internal class DefaultPolicy : CompletePolicy
    {
        private readonly BankingSystemSchedule _systemSchedule;

        public DefaultPolicy(BankingSystemSchedule systemSchedule)
        {
            Argument.NotNull(systemSchedule, "systemSchedule");
            _systemSchedule = systemSchedule;
        }

        public override bool AppliesTo(Transaction transaction, BankOperation operation)
        {
            return true;
        }

        public override int Priority
        {
            get { return int.MaxValue; }
        }

        public override DateTime GetPostDateUtc(Transaction transaction, BankOperation operation)
        {
            return _systemSchedule.GetPostDateUtc();
        }
    }
}
