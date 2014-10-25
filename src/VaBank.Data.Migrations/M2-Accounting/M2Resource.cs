using System;

namespace VaBank.Data.Migrations
{
    internal class M2Resource
    {
        public static M2Resource CreateWebServerFile(string uri)
        {
            return new M2Resource
            {
                Id = Guid.NewGuid(),
                Location = "WebServer",
                Type = "FileLink",
                Uri = uri
            };
        }

        public Guid Id { get; private set; }

        public string Type { get; private set; }

        public string Uri { get; private set; }

        public string Location { get; private set; }
    }
}
