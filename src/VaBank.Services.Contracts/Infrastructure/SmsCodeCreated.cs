using System;
using Newtonsoft.Json;
using VaBank.Common.Validation;
using VaBank.Services.Contracts.Common.Events;

namespace VaBank.Services.Contracts.Infrastructure
{
    public class SmsCodeCreated : ApplicationEvent
    {
        public SmsCodeCreated(Guid id, string code)
        {
            Argument.Satisfies(id, x => x != Guid.Empty);
            Argument.NotEmpty(code, "code");

            Id = id;
            Code = Code;
        }

        [JsonProperty("id")]
        public Guid Id { get; private set; }

        [JsonProperty("code")]
        public string Code { get; private set; }
    }
}
