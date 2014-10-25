using System;

namespace VaBank.Common.Resources
{
    public class WebServerUriProvider: IUriProvider
    {
        private readonly string _webServerUrl;

        public WebServerUriProvider(string webServerUrl)
        {
            _webServerUrl = webServerUrl ?? string.Empty;
            _webServerUrl = _webServerUrl.TrimEnd('/');
        }

        public string GetUri(string relativeUri, string location)
        {
            if (string.IsNullOrEmpty(relativeUri))
            {
                throw new ArgumentNullException("relativeUri");
            }
            return string.Format("{0}/{1}", _webServerUrl, relativeUri.TrimStart('/'));
        }

        public bool CanHandle(string location)
        {
            return location == "WebServer";
        }
    }
}