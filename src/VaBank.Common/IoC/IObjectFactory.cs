using System;

namespace VaBank.Common.IoC
{
    public interface IObjectFactory
    {
        object Create(Type objectType);
    }
}
