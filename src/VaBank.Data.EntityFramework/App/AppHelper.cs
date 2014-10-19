using System;
using VaBank.Core.App;

namespace VaBank.Data.EntityFramework.App
{
    internal static class AppHelper
    {
        public static DatabaseOperation ToOperation(this char value)
        {
            switch (value)
            {
                case 'U':
                    return DatabaseOperation.Update;
                case 'D':
                    return DatabaseOperation.Delete;
                case 'I':
                    return DatabaseOperation.Insert;
                default:
                    throw new InvalidOperationException("Can't convert char value to DatabaseOperation");
            }
        }
    }
}
