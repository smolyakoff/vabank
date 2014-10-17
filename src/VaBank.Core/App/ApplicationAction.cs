using System;
using VaBank.Core.Common;

namespace VaBank.Core.App
{
    public class ApplicationAction : Entity
    {
        protected ApplicationAction()
        {

        }
        
        public static ApplicationAction CreateAction(string code, Guid operationId, DateTime timestampUtc,
            string description = null, string data = null)
        {
            if (operationId == Guid.Empty)
                throw new ArgumentException("Operation id can't has empty value.");
            return new ApplicationAction
            {
                OperationId = operationId,
                EventId = Guid.NewGuid(),
                TimestampUtc = timestampUtc,
                Code = code,
                Description = description,
                Data = data
            };
        }        

        public Guid EventId { get; protected set; }

        public Guid OperationId { get; protected set; }

        public DateTime TimestampUtc { get; protected set; }

        public string Code { get; protected set; }

        public string Description { get; set; }

        public string Data { get; set; }
    }
}
