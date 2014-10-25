using System;

namespace VaBank.Common.Resources
{
    internal class DefaultUriProvider : IUriProvider
    {
        public string GetUri(string relativeUri, string location)
        {
            if (string.IsNullOrEmpty(relativeUri))
            {
                throw new ArgumentNullException("relativeUri");
            }
            return relativeUri;
        }

        public bool CanHandle(string location)
        {
            return !string.IsNullOrEmpty(location);
        }
    }
}
