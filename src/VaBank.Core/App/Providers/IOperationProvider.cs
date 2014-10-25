using VaBank.Core.App.Entities;

namespace VaBank.Core.App.Providers
{
    public interface IOperationProvider
    {
        bool HasCurrent { get; }

        Operation GetCurrent();
    }
}
