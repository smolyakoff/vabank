using System;
using Newtonsoft.Json;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Processing.Events
{
    public class OperationProgressEvent : ApplicationEvent, IBankOperationEvent
    {
        public OperationProgressEvent(Guid appOperationId, BankOperationModel bankOperation)
        {
            Argument.NotNull(bankOperation, "bankOperation");

            OperationId = appOperationId;
            BankOperationId = bankOperation.Id;
            Data = JsonConvert.SerializeObject(bankOperation);

            Code = FormatCode(bankOperation);
            Description = FormatDescription(bankOperation);
        }

        [JsonConstructor]
        protected OperationProgressEvent()
        {
        }

        [JsonProperty]
        public Guid OperationId { get; private set; }

        [JsonProperty]
        public long BankOperationId { get; private set; }

        [JsonProperty]
        public string Code { get; private set; }

        [JsonProperty]
        public string Description { get; private set; }

        [JsonProperty]
        public object Data { get; private set; }

        private static string FormatCode(BankOperationModel bankOperation)
        {
            const string pattern = "OP_{0}";
            var code = string.Format(pattern, bankOperation.CategoryCode.Replace('-','_')).ToUpperInvariant();
            return code;
        }

        private static string FormatDescription(BankOperationModel bankOperation)
        {
            var description = string.Format("Bank operation #{0}[{1}] is in progress.", bankOperation.Id,
                bankOperation.CategoryCode);
            return description;
        }
    }
}
