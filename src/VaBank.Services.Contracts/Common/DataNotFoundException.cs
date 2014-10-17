using System;
using VaBank.Services.Contracts.Common.Models;
using VaBank.Services.Contracts.Common.Resources;

namespace VaBank.Services.Contracts.Common
{
    public class DataNotFoundException : UserMessageException
    {
        public DataNotFoundException() : base(UserMessage.Resource(() => Messages.DataNotFound))
        {
        }

        public DataNotFoundException(Type objectType, object key) : base(UserMessage.ResourceFormat(() => Messages.TypedDataNotFound, objectType.Name, key))
        {
        }
    }
}
