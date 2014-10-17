using System;
using VaBank.Services.Contracts.Maintenance;

namespace VaBank.Jobs.Common
{
    public class DefaultJobContext<T> : DefaultJobContext, IJobContext<T>
    {
        public ILogManagementService LogManagementService { get; set; }

        public T Data { get; set; }
    }
}
