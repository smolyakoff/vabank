using System;
using System.Collections.Generic;
using System.Linq;

namespace VaBank.Core.App
{
    public class CompositeUriProvider : IUriProvider
    {
        private readonly List<IUriProvider> _uriProviders; 

        public CompositeUriProvider(IEnumerable<IUriProvider> uriProviders)
        {
            if (uriProviders == null)
            {
                throw new ArgumentNullException("uriProviders");
            }
            _uriProviders = uriProviders.ToList();
            _uriProviders.Add(new DefaultUriProvider());
        }

        public string GetAbsoluteUri(FileLink fileLink)
        {
            if (fileLink == null)
            {
                throw new ArgumentNullException("fileLink");
            }
            var handler = _uriProviders.First(x => x.CanHandle(fileLink));
            return handler.GetAbsoluteUri(fileLink);
        }

        public bool CanHandle(FileLink fileLink)
        {
            return fileLink != null;
        }
    }
}
