using System;

namespace VaBank.Core.App
{
    public class DefaultUriProvider : IUriProvider
    {
        public string GetAbsoluteUri(FileLink fileLink)
        {
            if (fileLink == null)
            {
                throw new ArgumentNullException("fileLink");
            }
            return fileLink.Uri;
        }

        public bool CanHandle(FileLink fileLink)
        {
            return fileLink != null;
        }
    }
}
