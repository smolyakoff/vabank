using System;

namespace VaBank.Services.Common.Validation
{
    public interface IObjectConverter
    {
        object Convert(object obj, Type destinationType);
    }
}
