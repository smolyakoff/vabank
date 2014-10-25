using System;
using System.Collections.Generic;
using System.Linq;

namespace VaBank.Common.Resources
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

        public string GetUri(string relativeUri, string location)
        {
            if (string.IsNullOrEmpty(relativeUri))
            {
                throw new ArgumentNullException("relativeUri");
            }
            var provider = _uriProviders.First(x => x.CanHandle(location));
            return provider.GetUri(relativeUri, location);
        }

        public bool CanHandle(string location)
        {
            return !string.IsNullOrEmpty(location);
        }
    }
}
