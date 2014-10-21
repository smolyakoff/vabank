namespace VaBank.Core.App
{
    public interface IUriProvider
    {
        string GetAbsoluteUri(FileLink fileLink);

        bool CanHandle(FileLink fileLink);
    }
}
