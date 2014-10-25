using System;

namespace VaBank.Common.Resources
{
    public class Link
    {
        public Link(string uri, string location)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException("uri");
            }
            Uri = uri;
            Location = location ?? "DefinedByUri";
        }

        protected Link()
        {
            
        }

        public string Uri { get; private set; }

        public string Location { get; private set; }
    }
}
