namespace VaBank.Core.App
{
    public interface IOperationProvider
    {
        bool HasCurrent { get; }

        Operation GetCurrent();
    }
}
