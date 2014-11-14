using System;
using Newtonsoft.Json;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Common.Events;

namespace VaBank.Services.Contracts.Infrastructure.Sms
{
    public class SmsCodeCreated : ApplicationEvent, ISmsEvent
    {
        public SmsCodeCreated(Guid id, string code)
        {
            Argument.Satisfies(id, x => x != Guid.Empty);
            Argument.NotEmpty(code, "code");

            Id = id;
            Code = code;
        }

        [JsonConstructor]
        protected SmsCodeCreated()
        {
        }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public string Code { get; private set; }
    }
}
