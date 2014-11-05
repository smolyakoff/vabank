using VaBank.Services.Common;

namespace VaBank.Services.Infrastructure.Email
{
    public class EmailServiceDependencies : IDependencyCollection
    {
        public SendGridClientFactory SendGridClientFactory { get; set; }
    }
}
