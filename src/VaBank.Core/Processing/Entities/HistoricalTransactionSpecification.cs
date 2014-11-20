using System;
using VaBank.Common.Data.Linq;
using VaBank.Common.Validation;
using VaBank.Core.Common.History;

namespace VaBank.Core.Processing.Entities
{
    public class HistoricalTransactionSpecification : IHistoricalEntitySpecification<HistoricalTransaction>
    {
        public LinqSpec<HistoricalTransaction> OriginalKey(params object[] keys)
        {
            Argument.NotNull(keys, "keys");
            Argument.Satisfies(keys, x => x.Length == 1, "keys");
            Argument.Satisfies(keys[0], x => x is Guid);

            return LinqSpec.For<HistoricalTransaction>(x => x.Id == (Guid)keys[0]);
        }
    }
}
