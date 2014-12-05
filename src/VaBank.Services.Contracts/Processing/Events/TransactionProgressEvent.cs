using System;
using Newtonsoft.Json;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Processing.Events
{
    public class TransactionProgressEvent : ApplicationEvent, ITransactionEvent
    {
        public TransactionProgressEvent(Guid operationId, TransactionModel transaction, long? bankOperationId = null)
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
        protected TransactionProgressEvent()
        {
        }

        [JsonProperty]
        public Guid OperationId { get;  set; }

        [JsonProperty]
        public Guid TransactionId { get;  set; }

        [JsonProperty]
        public long? BankOperationId { get;  set; }

        [JsonProperty]
        public string Code { get;  set; }

        [JsonProperty]
        public string Description { get;  set; }

        [JsonProperty]
        public object Data { get;  set; }

         static string FormatCode(TransactionModel transaction, long? bankOperationId)
        {
            const string pattern = "TRAN_{0}";
            var prefix = bankOperationId != null ? "OP_" : string.Empty;
            var code = string.Format(prefix + pattern, transaction.Code).ToUpperInvariant();
            return code;
        }

         static string FormatDescription(TransactionModel transaction, long? bankOperationId)
        {
            const string operationalPattern = "Transaction #{0}({1})[OP-{2}] is in progress.";
            const string pattern = "Transaction #{0}({1}) was changed.";
            var description = bankOperationId == null
                ? string.Format(pattern, transaction.Id, transaction.Description)
                : string.Format(operationalPattern, transaction.Id, transaction.Description, bankOperationId);
            return description;
        }
    }
}
