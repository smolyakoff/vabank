namespace VaBank.Common.Resources
{
    public interface IUriProvider
    {
        string GetUri(string relativeUri, string location);

        bool CanHandle(string location);
    }
}
