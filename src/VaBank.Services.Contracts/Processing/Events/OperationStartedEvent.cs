using System;
using Newtonsoft.Json;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Common.Events;
using VaBank.Services.Contracts.Processing.Models;

namespace VaBank.Services.Contracts.Processing.Events
{
    //TODO: attributes for serialization and constructor
    //TODO: maybe base class
    public class OperationStartedEvent : ApplicationEvent, IBankOperationEvent
    {
        public OperationStartedEvent(Guid operationId, BankOperationModel operationModel)
        {
            Argument.NotNull(operationModel, "operationModel");

            OperationId = operationId;
            BankOperationId = operationModel.Id;
            Data = JsonConvert.SerializeObject(operationModel);
            Code = string.Format("OP_STARTED_{0}", operationModel.CategoryCode.Replace('-', '_'));
        }

        public long BankOperationId { get; set; }

        public Guid OperationId { get; private set; }

        public string Code { get; private set; }

        public string Description { get; private set; }

        public object Data { get; private set; }
    }
}
