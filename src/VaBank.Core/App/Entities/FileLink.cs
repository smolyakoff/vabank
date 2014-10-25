using System;

namespace VaBank.Core.App.Entities
{
    public class FileLink : Resource
    {
        public static FileLink Create(Uri uri, FileLinkLocation location)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            var uriString = uri.ToString();
            return new FileLink {Location = location.ToString(), Uri = uriString};
        }

        public string Uri { get; protected set; }
    }
}
