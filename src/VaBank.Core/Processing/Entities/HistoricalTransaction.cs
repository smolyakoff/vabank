using System;
using System.Linq.Expressions;
using VaBank.Common.Validation;
using VaBank.Core.Accounting.Entities;
using VaBank.Core.App.Entities;
using VaBank.Core.Common;

namespace VaBank.Core.Processing.Entities
{
    public class HistoricalTransaction : ITransaction, IHistoricalEntity<HistoricalTransaction>
    {
        public long HistoryId { get; internal set; }

        public char HistoryAction { get; internal set; }

        public DateTime HistoryTimestampUtc { get; internal set; }

        public Guid HistoryOperationId { get; internal set; }

        public virtual Operation HistoryOperation { get; internal set; }

        public Expression<Func<HistoricalTransaction, bool>> GetSurrogateKeyFilterExpression(object surrogateKey)
        {
            Argument.NotNull(surrogateKey, "surrogateKey");

            var parameter = Expression.Parameter(surrogateKey.GetType(), "x");
            Expression body = Expression.Property(parameter, "Id");
            body = Expression.Equal(body, Expression.Constant(surrogateKey));
            return Expression.Lambda<Func<HistoricalTransaction, bool>>(body, parameter);
        }

        public string AccountNo { get; internal set; }

        public virtual Currency Currency { get; internal set; }

        public decimal TransactionAmount { get; internal set; }

        public decimal AccountAmount { get; internal set; }

        public decimal RemainingBalance { get; internal set; }

        public DateTime CreatedDateUtc { get; internal set; }

        public DateTime? PostDateUtc { get; internal set; }

        public string Description { get; internal set; }

        public string Location { get; internal set; }

        public string ErrorMessage { get; internal set; }

        public ProcessStatus Status { get; internal set; }

        public Guid Id { get; internal set; }
    }
}
