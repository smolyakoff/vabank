using VaBank.Common.Util;

namespace VaBank.Services.Infrastructure.Email
{
    [Settings("VaBank.Services.Infrastructure.Email.SendGridClientSettings")]
    public class SendGridClientSettings
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
