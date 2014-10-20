using System;

namespace VaBank.Common.Data
{
    public interface IObjectConverter
    {
        object Convert(object obj, Type destinationType);
    }
}
