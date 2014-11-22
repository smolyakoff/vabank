using System;
using Newtonsoft.Json;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Processing.Events
{
    public class TransactionChangedEvent : ApplicationEvent, ITransactionEvent
    {
        public TransactionChangedEvent(Guid operationId, TransactionModel transaction, long? bankOperationId = null)
        {
            Argument.NotNull(transaction, "transaction");
            Argument.Satisfies(operationId, x => x != Guid.Empty);
            OperationId = operationId;
            TransactionId = transaction.Id;
            BankOperationId = bankOperationId;

            Data = JsonConvert.SerializeObject(transaction);
            Code = FormatCode(transaction, bankOperationId);
            Description = FormatDescription(transaction, bankOperationId);
        }

        [JsonConstructor]
        protected TransactionChangedEvent()
        {
        }

        [JsonProperty]
        public Guid OperationId { get; private set; }

        [JsonProperty]
        public Guid TransactionId { get; private set; }

        [JsonProperty]
        public long? BankOperationId { get; private set; }

        [JsonProperty]
        public string Code { get; private set; }

        [JsonProperty]
        public string Description { get; private set; }

        [JsonProperty]
        public object Data { get; private set; }

        private static string FormatCode(TransactionModel transaction, long? bankOperationId)
        {
            const string pattern = "TRAN_{0}";
            var prefix = bankOperationId != null ? "OP_" : string.Empty;
            var code = string.Format(prefix + pattern, transaction.Status);
            return code;
        }

        private static string FormatDescription(TransactionModel transaction, long? bankOperationId)
        {
            const string operationalPattern = "Transaction #{0}({1})[OP-{2}] was changed.";
            const string pattern = "Transaction #{0}({1}) was changed.";
            var description = bankOperationId == null
                ? string.Format(pattern, transaction.Id, transaction.Description)
                : string.Format(operationalPattern, transaction.Id, transaction.Description, bankOperationId);
            return description;
        }
    }
}
