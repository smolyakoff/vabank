using System;

namespace VaBank.Services.Common
{
    public interface IObjectFactory
    {
        object Create(Type objectType);
    }
}
