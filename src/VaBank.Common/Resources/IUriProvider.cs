namespace VaBank.Common.Resources
{
    public interface IUriProvider
    {
        string GetAbsoluteUri(string relativeUri, string location);

        bool CanHandle(string location);
    }
}
