using System;
using VaBank.Common.IoC;
using VaBank.Common.Resources;
using VaBank.Core.App.Entities;

namespace VaBank.Core.App.Providers
{
    [Injectable(Lifetime = Lifetime.Singleton)]
    public class FileLinkUriProvider
    {
        private readonly IUriProvider _uriProvider;

        public FileLinkUriProvider(IUriProvider uriProvider)
        {
            if (uriProvider == null)
            {
                throw new ArgumentNullException("uriProvider");
            }
            _uriProvider = uriProvider;
        }

        public string GetAbsuluteUri(FileLink fileLink)
        {
            if (!_uriProvider.CanHandle(fileLink.Location))
            {
                var message = string.Format("Location [{0}] is not supported.", fileLink.Location);
                throw new NotSupportedException(message);
            }
            return _uriProvider.GetAbsoluteUri(fileLink.Uri, fileLink.Location);
        }
    }
}
